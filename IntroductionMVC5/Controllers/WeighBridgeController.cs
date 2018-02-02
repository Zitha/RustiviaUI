using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web.Mvc;
using IntroductionMVC5.Data;
using IntroductionMVC5.Models;
using IntroductionMVC5.Models.Integrator;
using IntroductionMVC5.Web.Utils;
using IntroductionMVC5.Web.Utils.PagedList;
using IntroductionMVC5.Web.Utils.Printing;
using IntroductionMVC5.Web.ViewModel;

namespace IntroductionMVC5.Web.Controllers
{
    [Authorize]
    public class WeighBridgeController : PdfViewController
    {
        private readonly int _defaultPageSize =
            Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPaginationSize"]);

        private readonly ApplicationUnit _unit = new ApplicationUnit();

        public ActionResult Index(int? page)
        {
            //            List<WeighBridgeInfo> weighBridge = _unit.WeighBridgeInfos
            //                .GetAll().Include(a => a.Driver).Include(p => p.PastelInfo).OrderByDescending(s => s.Id).ToList();

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            //            IPagedList<WeighBridgeInfo> providersListPaged = weighBridge.ToPagedList();// currentPageIndex, defaultPageSize);

            return View();
        }

        [HttpPost]
        public ActionResult Process(ProcessViewModel processViewModel)
        {
            //Pastel File location Info
            string psourcePath = string.Empty;

            //Get WeighBridge File
            string vsourcePath;
            //Get Pastel Files
            psourcePath = MovePastelFile(processViewModel.PastelInfo);
            //Get Vat Form
            if (Request.Files.Count > 0 && Request.Files["VatFile"] != null
                && !string.IsNullOrEmpty(Request.Files["VatFile"].FileName))
            {
                vsourcePath = MoveVatForm(processViewModel.PastelInfo);
            }
            else
            {
                vsourcePath = processViewModel.PastelInfo.VatFile;
            }

            ICollection<string> keys = ModelState.Keys;
            foreach (string key in keys)
            {
                if (key.Contains("Driver"))
                {
                    ModelState[key].Errors.Clear();
                }
            }

            Purchase purchase = _unit.Purchase.GetAll().Include(a => a.Driver).Include(w => w.WeighBridgeInfo)
                .FirstOrDefault(x => x.WeighBridgeInfo.Id == processViewModel.Purchase.WeighBridgeInfo.Id);
            if (purchase != null)
            {
                purchase.Status = Statuses.Processed;
                purchase.PaymentReference = processViewModel.Purchase.PaymentReference;
                purchase.PaymentType = processViewModel.Purchase.PaymentType;
                processViewModel.PastelInfo.WeighBridgeInfo = purchase.WeighBridgeInfo;
                processViewModel.PastelInfo.FileLocation = psourcePath;
                processViewModel.PastelInfo.VatFile = vsourcePath;
                processViewModel.PastelInfo.Date = DateTime.Now;
            }
            ModelState["PastelInfo.WeighBridgeInfo"].Errors.Clear();
            ModelState["SupplierInfo.CompanyRegNumber"].Errors.Clear();
            if (ModelState.IsValid)
            {
                _unit.PastelInfos.Add(processViewModel.PastelInfo);
                _unit.Purchase.Update(purchase);
                _unit.SaveChanges();

                return RedirectToAction("UnProcessed");
            }

            return View(processViewModel);
        }

        private string MovePastelFile(PastelInfo pastelInfo)
        {
            string psourcePath = "";
            string pastelDestination = ConfigurationManager.AppSettings["PastelFiles"];
            if (!Directory.Exists(Server.MapPath("~/App_Data" + pastelDestination)))
            {
                Directory.CreateDirectory(Server.MapPath("~/App_Data" + pastelDestination));
            }

            if (Request.Files.Count > 0 && Request.Files["PastelFile"] != null
                && !string.IsNullOrEmpty(Request.Files["PastelFile"].FileName))
            {
                string pastelFileName = Request.Files["PastelFile"].FileName;
                string extension = Path.GetExtension(pastelFileName);
                psourcePath = Path.Combine(Server.MapPath("~/App_Data" + pastelDestination),
                    string.Format("{0}{1}", pastelInfo.PastelNumber, extension));

                Request.Files["PastelFile"].SaveAs(psourcePath);
                pastelInfo.Date = DateTime.Now.Date;
                ModelState.SetModelValue("PastelInfo.FileLocation",
                    new ValueProviderResult(new List<string>
                    {
                        psourcePath
                    }, psourcePath, CultureInfo.CurrentCulture));

                ModelState["PastelInfo.FileLocation"].Errors.Clear();
            }
            return psourcePath;
        }

        private string MoveVatForm(PastelInfo pastelInfo)
        {
            string vatSourcePath = "";
            string vatFormLocation = ConfigurationManager.AppSettings["VatFiles"];
            if (!Directory.Exists(Server.MapPath("~/App_Data" + vatFormLocation)))
            {
                Directory.CreateDirectory(Server.MapPath("~/App_Data" + vatFormLocation));
            }
            string idFileName = Request.Files["VatFile"].FileName;
            string extension = Path.GetExtension(idFileName);
            vatSourcePath = Path.Combine(Server.MapPath("~/App_Data" + vatFormLocation),
                string.Format("{0}_VATFILE{1}", pastelInfo.PastelNumber, extension));
            Request.Files["VatFile"].SaveAs(vatSourcePath);
            ModelState.SetModelValue("VatFile",
                new ValueProviderResult(new List<string>
                {
                    vatSourcePath
                }, vatSourcePath, CultureInfo.CurrentCulture));

            ModelState["VatFile"].Errors.Clear();

            return vatSourcePath;
        }

        [HttpGet]
        public ActionResult Process(int? id)
        {
            //            ConvertToPDFNow();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = _unit.Purchase.GetAll().Include(a => a.WeighBridgeInfo)
                .Include(d => d.Driver).FirstOrDefault(x => x.WeighBridgeInfo.Id == id);

            List<SupplierInfo> clientInfos = _unit.SupplierInfo
                .GetAll().Include(d => d.Drivers).ToList();
            SupplierInfo supplier = null;

            foreach (SupplierInfo cl in clientInfos)
            {
                if (cl.Drivers.Any(driver => purchase != null && driver.Id == purchase.Driver.Id))
                {
                    supplier = cl;
                }
            }
            var processViewModel = new ProcessViewModel
            {
                SupplierInfo = supplier,
                Purchase = purchase
            };

            return View(processViewModel);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = _unit.Purchase.GetAll()
                .Include(w => w.WeighBridgeInfo)
                .Include(d => d.Driver)
                .FirstOrDefault(x => x.WeighBridgeInfo.Id == id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(Purchase puchase)
        {
            Purchase purchase = _unit.Purchase.GetAll()
                .Include(d => d.Driver).Include(w => w.WeighBridgeInfo)
                .FirstOrDefault(x => x.WeighBridgeInfo.Id == puchase.WeighBridgeInfo.Id);

            purchase.Status = Statuses.Deleted;
            purchase.WeighBridgeInfo.Comments = puchase.WeighBridgeInfo.Comments;
            _unit.Purchase.Update(purchase);
            _unit.SaveChanges();
            return RedirectToAction("UnProcessed");
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

        public ActionResult WeighBridgePrint(int id)
        {
            Purchase purchase = _unit.Purchase.GetAll()
                .Include(a => a.Driver)
                .Include(x => x.Truck)
                .Include(x => x.WeighBridgeInfo)
                .FirstOrDefault(x => x.WeighBridgeInfo.Id == id);

            return ViewPdf(string.Empty, "WeighBridgePrint", purchase);
        }

        public ActionResult Edit(int id)
        {
            PastelInfo pastelInfo = _unit.PastelInfos.GetAll()
                .Include(a => a.WeighBridgeInfo)
                .FirstOrDefault(x => x.Id == id);

            return View(pastelInfo);
        }

        [HttpPost]
        public ActionResult Edit(PastelInfo pastelInfo)
        {
            PastelInfo pastelInfoToUpdate = _unit.PastelInfos.GetAll()
                .Include(a => a.WeighBridgeInfo)
                .FirstOrDefault(x => x.Id == pastelInfo.Id);
            //getPastelFile
            string psourcePath = string.Empty;
            if (Request.Files.Count > 0 && Request.Files["PastelFile"] != null
                && !string.IsNullOrEmpty(Request.Files["PastelFile"].FileName))
            {
                DeleteFile(pastelInfoToUpdate.FileLocation);
                psourcePath = MovePastelFile(pastelInfo);
            }
            //Get Vat Form
            string vsourcePath = string.Empty;
            if (Request.Files.Count > 0 && Request.Files["VatFile"] != null
                && !string.IsNullOrEmpty(Request.Files["VatFile"].FileName))
            {
                DeleteFile(pastelInfoToUpdate.VatFile);
                vsourcePath = MoveVatForm(pastelInfo);
            }

            if (pastelInfoToUpdate != null)
            {
                pastelInfoToUpdate.VatFile = string.IsNullOrEmpty(vsourcePath)
                    ? pastelInfoToUpdate.VatFile
                    : vsourcePath;
                pastelInfoToUpdate.PastelNumber = pastelInfo.PastelNumber;
                pastelInfoToUpdate.FileLocation = string.IsNullOrEmpty(psourcePath)
                    ? pastelInfoToUpdate.FileLocation
                    : psourcePath;
            }

            _unit.PastelInfos.Update(pastelInfoToUpdate);
            _unit.SaveChanges();

            return RedirectToAction("Index");
        }

        private void DeleteFile(string flieLocation)
        {
            if (System.IO.File.Exists(flieLocation))
            {
                System.IO.File.Delete(flieLocation);
            }
        }

        public ViewResultBase Search(DateTime to, DateTime from, int? page)
        {
            var weighBridgeInfos = new List<WeighBridgeInfo>();
            //                _unit.WeighBridgeInfos.GetAll().Include(r => r.Driver).Include(p => p.PastelInfo).ToList();

            //alfabet, first letter
            weighBridgeInfos = _unit.WeighBridgeInfos.GetAll().Where(x => x.DateIn >= from && x.DateIn <= to).ToList();

            //            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            //            IPagedList<WeighBridgeInfo> providersListPaged = weighBridgeInfos.OrderByDescending(i => i.Id)
            //                .ToPagedList(currentPageIndex, defaultPageSize);

            if (Request.IsAjaxRequest())
                return PartialView("Index", weighBridgeInfos);
            return View("Index", weighBridgeInfos);
        }

        public ActionResult UnProcessed()
        {
            List<Purchase> purchases = _unit.Purchase.GetAll()
                .OrderByDescending(i => i.WeighBridgeInfo.Id)
                .Include(d => d.Driver).Include(w => w.WeighBridgeInfo)
                .Include(t => t.Truck).Where(w => w.Status == Statuses.Unprocessed).ToList();
            ViewBag.Product = _unit.Product.GetAll().ToList();

            return View(purchases);
        }

        public ActionResult Processed(int? page)
        {
            List<Purchase> purchases = _unit.Purchase.GetAll()
                .OrderByDescending(i => i.WeighBridgeInfo.Id)
                .Include(d => d.Driver)
                .Include(w => w.WeighBridgeInfo)
                .Include(t => t.Truck)
                .Where(w => w.Status == Statuses.Processed).ToList();

            List<PastelInfo> pastelInfos =
                (from pastelInfo in _unit.PastelInfos.GetAll().Include(i => i.WeighBridgeInfo)
                    .OrderByDescending(i => i.WeighBridgeInfo.Id).ToList()
                 join w in purchases on pastelInfo.WeighBridgeInfo.Id equals w.WeighBridgeInfo.Id
                 select pastelInfo).ToList();

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            IPagedList<PastelInfo> providersListPaged = pastelInfos.ToPagedList(currentPageIndex,
                _defaultPageSize);

            return View("Processed", providersListPaged);
        }

        public ActionResult Deleted(int? page)
        {
            List<Purchase> purchase = _unit.Purchase.GetAll()
                .OrderByDescending(i => i.WeighBridgeInfo.Id)
                .Include(a => a.Driver)
                .Include(x => x.WeighBridgeInfo)
                .Where(w => w.Status == Statuses.Deleted).ToList();

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            IPagedList<Purchase> providersListPaged = purchase.ToPagedList(currentPageIndex,
                _defaultPageSize);

            return View(providersListPaged);
        }
    }
}