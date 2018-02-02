using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web.Mvc;
using IntroductionMVC5.Data;
using IntroductionMVC5.Models;
using IntroductionMVC5.Models.Integrator;
using IntroductionMVC5.Web.Models;
using IntroductionMVC5.Web.Utils;
using IntroductionMVC5.Web.Utils.PagedList;
using IntroductionMVC5.Web.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IntroductionMVC5.Web.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {
        private readonly ApplicationDbContext _appContext = new ApplicationDbContext();

        private readonly int _defaultPageSize =
            Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPaginationSize"]);

        private readonly ApplicationUnit _unit = new ApplicationUnit();
        private List<SupplierInfo> _suppliers = new List<SupplierInfo>();

        public SupplierController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
            _suppliers = GetAllSuppliers();
        }

        public SupplierController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        private List<SupplierInfo> GetAllSuppliers()
        {
            List<SupplierInfo> suppliers = _unit.SupplierInfo.GetAll()
                .OrderBy(s => s.SupplierName).ToList();
            return suppliers;
        }

        [HttpGet]
        public ActionResult Index(int? page)
        {
            var settings =
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
            string jsonSuppliers = JsonConvert.SerializeObject(_suppliers, settings);
            ViewBag.SupplierList = jsonSuppliers;

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            IPagedList<SupplierInfo> providersListPaged = _suppliers.ToPagedList(currentPageIndex,
                _defaultPageSize);
            return View(providersListPaged);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var supplier = new SupplierInfo
            {
                Logo = "Supplier.png"
            };
            return View(supplier);
        }

        public ActionResult Details(int id)
        {
            SupplierInfo supplier = _unit.SupplierInfo.GetAll()
                .Include(a => a.Drivers).Include(t => t.Trucks).FirstOrDefault(a => a.Id == id);

            var purchases = new List<Purchase>();
            if (supplier != null)
                foreach (Driver driver in supplier.Drivers)
                {
                    List<Purchase> pur = _unit.Purchase.GetAll().Include(d => d.Driver).Include(w => w.WeighBridgeInfo)
                        .Where(w => w.Driver.Id == driver.Id && w.Status == Statuses.Processed).ToList();
                    purchases.AddRange(pur);
                }

            var pastelInfos = new List<PastelInfo>();
            if (purchases.Count > 0)
            {
                pastelInfos = (from pastelInfo in _unit.PastelInfos.GetAll().Include(w => w.WeighBridgeInfo).ToList()
                               join w in purchases on pastelInfo.WeighBridgeInfo.Id equals w.WeighBridgeInfo.Id
                               select pastelInfo).ToList();
            }

            List<SupplierProduct> supplierProducts =
                _unit.SupplierProduct.GetAll().Where(sp => sp.SupplierId == id).ToList();

            List<BankAccount> supplierBankAccounts =
                _unit.BankAccounts.GetAll().Where(sp => sp.SupplierInfo.Id == id).ToList();

            var supplierViewModel = new SupplierDetailsViewModel
            {
                SupplierInfo = supplier,
                Purchases = purchases,
                PastelInfos = pastelInfos,
                SupplierProducts = supplierProducts,
                SupplierBankAccounts = supplierBankAccounts
            };

            ViewBag.UserManager = UserManager;
            ViewBag.Products = new SelectList(_unit.Product.GetAll().OrderBy(d => d.Name).ToList(), "Id", "Name");

            return View(supplierViewModel);
        }

        public ActionResult GetPdf(string fileName)
        {
            using (var webClient = new WebClient())
            {
                if (!System.IO.File.Exists(fileName))
                {
                    return HttpNotFound();
                }
                byte[] file = webClient.DownloadData(fileName);
                return File(file, MediaTypeNames.Application.Pdf);
            }
        }

        [HttpPost]
        public ActionResult Create(SupplierInfo supplierInfo)
        {
            if (ModelState.IsValid)
            {
                supplierInfo.SupplierName = supplierInfo.SupplierName.ToUpper();
                supplierInfo.Suppliercode = supplierInfo.Suppliercode.ToUpper();
                supplierInfo.Address = supplierInfo.Address.ToUpper();
                _unit.SupplierInfo.Add(supplierInfo);
                _unit.SaveChanges();
                _suppliers = GetAllSuppliers();
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            SupplierInfo supplierInfo = _unit.SupplierInfo.GetAll()
                .FirstOrDefault(x => x.Id == id);
            ViewBag.Users = new SelectList(_appContext.Users.OrderBy(d => d.UserName).ToList(), "Id", "UserName");
            return View(supplierInfo);
        }

        [HttpPost]
        public ActionResult Edit(SupplierInfo supplierInfo)
        {
            SupplierInfo supplierInfoToUpdate = _unit.SupplierInfo.GetAll()
                .FirstOrDefault(x => x.Id == supplierInfo.Id);

            if (supplierInfoToUpdate != null)
            {
                supplierInfoToUpdate.SupplierName = !string.IsNullOrEmpty(supplierInfo.SupplierName)
                    ? supplierInfo.SupplierName
                    : supplierInfoToUpdate.SupplierName;
                supplierInfoToUpdate.Suppliercode = !string.IsNullOrEmpty(supplierInfo.Suppliercode)
                    ? supplierInfo.Suppliercode
                    : supplierInfoToUpdate.Suppliercode;
                supplierInfoToUpdate.CompanyRegNumber = !string.IsNullOrEmpty(supplierInfo.CompanyRegNumber)
                    ? supplierInfo.CompanyRegNumber
                    : supplierInfoToUpdate.CompanyRegNumber;
                supplierInfoToUpdate.VatNumber = !string.IsNullOrEmpty(supplierInfo.VatNumber)
                    ? supplierInfo.VatNumber
                    : supplierInfoToUpdate.VatNumber;
                supplierInfoToUpdate.UserId = !string.IsNullOrEmpty(supplierInfo.UserId)
                    ? supplierInfo.UserId
                    : supplierInfoToUpdate.UserId;
                supplierInfoToUpdate.Suppliercode = supplierInfo.Suppliercode;
                supplierInfoToUpdate.IsBlocked = supplierInfo.IsBlocked;
            }
            _unit.SupplierInfo.Update(supplierInfoToUpdate);
            _unit.SaveChanges();

            return RedirectToAction("Details", new { id = supplierInfo.Id });
        }

        [HttpPost]
        public ActionResult AddProduct(SupplierProduct product, int supplierId)
        {
            SupplierInfo supplier = _unit.SupplierInfo.GetAll().FirstOrDefault(d => d.Id == supplierId);
            Product selectedProduct = _unit.Product.GetAll().FirstOrDefault(d => d.Id == product.ProductId);
            if (selectedProduct != null && supplier != null)
            {
                List<SupplierProduct> supplierProd =
                    _unit.SupplierProduct.GetAll().Where(d => d.SupplierId == supplierId).ToList();
                if (supplierProd.All(sp => sp.ProductId != product.ProductId))
                {
                    var supplierProduct =
                        new SupplierProduct
                        {
                            SupplierId = supplier.Id,
                            ProductId = product.ProductId,
                            SupplierPrice = product.SupplierPrice
                        };
                    _unit.SupplierProduct.Add(supplierProduct);
                    _unit.SaveChanges();
                }
                else
                {
                    ViewBag.ErrorMessage = "Supplier Arealdy Has Selected Product";
                }
            }
            return RedirectToAction("Details", new { id = supplierId });
        }

        [HttpPost]
        public ActionResult AddBankAccount(BankAccount bankAccount, int supplierId)
        {
            SupplierInfo supplier = _unit.SupplierInfo.GetAll().FirstOrDefault(d => d.Id == supplierId);
            if (supplier != null)
            {
                bankAccount.SupplierInfo = supplier;

                _unit.BankAccounts.Add(bankAccount);
                _unit.SaveChanges();
            }
            return RedirectToAction("Details", new { id = supplierId });
        }

        public ViewResultBase Search(string search)
        {
            const int currentPageIndex = 0;
            List<SupplierInfo> supplierInfos =
                _unit.SupplierInfo.GetAll().OrderBy(s => s.SupplierName).ToList();

            //alfabet, first letter
            if (!string.IsNullOrEmpty(search))
            {
                supplierInfos = supplierInfos.OrderBy(s => s.SupplierName)
                    .Where(s => s.SupplierName.ToUpper().Contains(search.ToUpper())).ToList();
            }
            IPagedList<SupplierInfo> providersListPaged = supplierInfos.ToPagedList(currentPageIndex,
                _defaultPageSize);

            if (Request.IsAjaxRequest())
                return PartialView("Index", providersListPaged);
            return View("Index", providersListPaged);
        }

        public ActionResult SalesReps()
        {
            List<ApplicationUser> sales = _appContext.Users.ToList();
            return View(sales);
        }

        public ActionResult EditSupplierProduct(SupplierProduct supplierProduct)
        {
            var updateProduct = _unit.SupplierProduct.GetAll().FirstOrDefault(p => p.ProductId == supplierProduct.ProductId
                && p.SupplierId == supplierProduct.SupplierId);
            if (updateProduct != null)
            {
                updateProduct.SupplierPrice = supplierProduct.SupplierPrice;
                _unit.SupplierProduct.Update(updateProduct);
            }
            _unit.SaveChanges();

            return RedirectToAction("Details", new { id = supplierProduct.SupplierId });
        }

        public ActionResult EditSupplierBankAccount(BankAccount bankAccount)
        {
            var updateBankAccount = _unit.BankAccounts.GetAll().FirstOrDefault(p => p.Id == bankAccount.Id);
            if (updateBankAccount != null)
            {
                updateBankAccount.BankName = bankAccount.BankName;
                updateBankAccount.AccountNumber = bankAccount.AccountNumber;
                updateBankAccount.BranchNumber = bankAccount.BranchNumber;
                _unit.BankAccounts.Update(updateBankAccount);
                _unit.SaveChanges();
            }

            return RedirectToAction("Details", new { id = updateBankAccount.SupplierInfo.Id });
        }
    }
}