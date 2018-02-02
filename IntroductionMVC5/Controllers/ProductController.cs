using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IntroductionMVC5.Data;
using IntroductionMVC5.Models;
using IntroductionMVC5.Models.Integrator;
using IntroductionMVC5.Web.Models;
using IntroductionMVC5.Web.Utils;
using IntroductionMVC5.Web.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IntroductionMVC5.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationUnit _unit = new ApplicationUnit();
        private readonly ApplicationDbContext _appContext = new ApplicationDbContext();
        public UserManager<ApplicationUser> UserManager { get; private set; }

        public ProductController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public ProductController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        // GET: Sales
        public ActionResult Index()
        {
            var products = _unit.Product.GetAll().ToList();
            return View(products);
        }

        public ActionResult SalesRep(string userId)
        {
            var sales = new List<SalesRepViewModel>();
            var salesReps = _appContext.Users.FirstOrDefault(usr => usr.Id == userId);

            var supplier = _unit.SupplierInfo.GetAll().Where(d => d.UserId == userId)
                .Include(a => a.Drivers).ToList();

            foreach (var supplierInfo in supplier)
            {
                var purchases = new List<Purchase>();
                foreach (var driver in supplierInfo.Drivers)
                {
                    purchases = _unit.Purchase.GetAll().Include(d => d.Driver).Include(w => w.WeighBridgeInfo)
                     .Where(w => w.Driver.Id == driver.Id && w.Status == Statuses.Processed).ToList();
                    purchases.AddRange(purchases);
                }

                sales.Add(new SalesRepViewModel
                {
                    ApplicationUser = salesReps,
                    Supplier = supplierInfo,
                    Purchases = purchases
                });
            }

            return View(sales);
        }

        public ActionResult AssignSalesRap()
        {
            List<SupplierInfo> supplierInfos = _unit.SupplierInfo.GetAll().OrderBy(d => d.SupplierName).ToList();
            ViewBag.UserManager = UserManager;
            ViewBag.Users = new SelectList(_appContext.Users.OrderBy(d => d.UserName).ToList(), "Id", "UserName");
            return View(supplierInfos);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                if (!_unit.Product.GetAll().Any(p => p.Name.ToUpper() == product.Name))
                {
                    product.Name = product.Name.ToUpper();
                    _unit.Product.Add(product);

                    _unit.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.ErrorMessage = "The Product You Are Trying To Add Exist In The Database";
            }
            return View();
        }

        //[HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                var updateProduct = _unit.Product.GetAll().FirstOrDefault(p => p.Id == product.Id);
                if (updateProduct != null)
                {
                    updateProduct.RustiviaPrice = product.RustiviaPrice;
                    _unit.Product.Update(updateProduct);
                }
                _unit.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult SpecialPrice(int productId)
        {
            var supplierProducts = _unit.SupplierProduct.GetAll()
                .Where(p => p.ProductId == productId).Include(d => d.Supplier).ToList();

            var supplierInfos = supplierProducts.Select(supplierProduct => supplierProduct.Supplier).ToList();
            ViewBag.ProductId = productId;

            return View(supplierInfos);
        }
    }
}