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
    public class ContainerController : PdfViewController
    {
        private readonly int _defaultPageSize =
            Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPaginationSize"]);

        private readonly ApplicationUnit _unit = new ApplicationUnit();

        //
        // GET: /Container/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UnProcessed()
        {
            List<Container> containers = _unit.Containers.GetAll()
                .OrderByDescending(i => i.WeighBridgeInfo.Id)
                .Include(d => d.Booking).Include(w => w.WeighBridgeInfo)
                .Where(w => w.Status == Statuses.Unprocessed).ToList();

            List<Sale> sales = _unit.Sale.GetAll()
                .OrderByDescending(i => i.WeighBridgeInfo.Id)
                .Include(w => w.WeighBridgeInfo)
                .Include(c => c.Customer)
                .Where(w => w.Status == Statuses.Unprocessed).ToList();

            var salesViewModel = new UprocessedSalesViewModel
            {
                Containers = containers,
                Sales = sales
            };

            return View(salesViewModel);
        }

        public ActionResult Processed(int? page)
        {
            List<Container> containers = _unit.Containers.GetAll()
                .OrderByDescending(i => i.WeighBridgeInfo.Id)
                .Include(w => w.WeighBridgeInfo)
                .Where(w => w.Status == Statuses.Processed).ToList();

            List<Sale> sales = _unit.Sale.GetAll()
                           .OrderByDescending(i => i.WeighBridgeInfo.Id)
                           .Include(w => w.WeighBridgeInfo)
                           .Where(w => w.Status == Statuses.Processed).ToList();

            List<WeighBridgeInfo> wbInfos = containers.Select(container => container.WeighBridgeInfo).ToList();
            wbInfos.AddRange(sales.Select(sale => sale.WeighBridgeInfo));

            List<PastelInfo> pastelInfos =
                (from pastelInfo in _unit.PastelInfos.GetAll().Include(i => i.WeighBridgeInfo)
                    .OrderByDescending(i => i.WeighBridgeInfo.Id).ToList()
                 join w in wbInfos on pastelInfo.WeighBridgeInfo.Id equals w.Id
                 select pastelInfo).ToList();

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            IPagedList<PastelInfo> providersListPaged = pastelInfos.ToPagedList(currentPageIndex,
                _defaultPageSize);

            return View("Processed", providersListPaged);
        }

        public ActionResult Deleted()
        {
            List<Container> containers = _unit.Containers.GetAll()
                .OrderBy(p => p.WeighBridgeInfo.Id)
                .Include(x => x.WeighBridgeInfo)
                .Include(x => x.Booking)
                .Where(w => w.Status == Statuses.Deleted).ToList();

            return View(containers);
        }

        public ActionResult WeighBridgePrint(int id)
        {
            Container container = _unit.Containers.GetAll()
                .Include(a => a.Booking)
                .Include(x => x.WeighBridgeInfo)
                .FirstOrDefault(x => x.WeighBridgeInfo.Id == id);

            return ViewPdf(string.Empty, "_SalePrint", container);
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

        [HttpGet]
        public ActionResult Process(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Container container = _unit.Containers.GetAll().Include(a => a.WeighBridgeInfo)
                .Include(d => d.Booking).FirstOrDefault(x => x.WeighBridgeInfo.Id == id);

            Sale sale = _unit.Sale.GetAll().Include(a => a.WeighBridgeInfo)
                .Include(d => d.Customer).FirstOrDefault(x => x.WeighBridgeInfo.Id == id);

            var processViewModel = new ProcessViewModel
            {
                Container = container,
                Sale = sale
            };

            return View(processViewModel);
        }

        [HttpPost]
        public ActionResult Process(ProcessViewModel processViewModel)
        {
            //Pastel File location Info
            string psourcePath;

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
                vsourcePath = processViewModel.PastelInfo.VatFile ?? "";
            }

            ICollection<string> keys = ModelState.Keys;
            foreach (string key in keys)
            {
                if (key.Contains("Driver"))
                {
                    ModelState[key].Errors.Clear();
                }
            }

            if (processViewModel.Container != null)
            {
                Container container = _unit.Containers.GetAll().Include(a => a.Booking).Include(w => w.WeighBridgeInfo)
               .FirstOrDefault(x => x.WeighBridgeInfo.Id == processViewModel.Container.WeighBridgeInfo.Id);
                if (container != null)
                {
                    container.Status = Statuses.Processed;
                    processViewModel.PastelInfo.WeighBridgeInfo = container.WeighBridgeInfo;
                    processViewModel.PastelInfo.FileLocation = psourcePath;
                    processViewModel.PastelInfo.VatFile = vsourcePath;
                    processViewModel.PastelInfo.Date = DateTime.Now;
                }
                ModelState["PastelInfo.WeighBridgeInfo"].Errors.Clear();
                if (ModelState.IsValid)
                {
                    _unit.PastelInfos.Add(processViewModel.PastelInfo);
                    _unit.Containers.Update(container);
                    _unit.SaveChanges();

                    return RedirectToAction("UnProcessed");
                }
            }
            else
            {
                Sale sale = _unit.Sale.GetAll().Include(a => a.Customer).Include(w => w.WeighBridgeInfo)
                    .FirstOrDefault(x => x.WeighBridgeInfo.Id == processViewModel.Sale.WeighBridgeInfo.Id);
                if (sale != null)
                {
                    sale.Status = Statuses.Processed;
                    processViewModel.PastelInfo.WeighBridgeInfo = sale.WeighBridgeInfo;
                    processViewModel.PastelInfo.FileLocation = psourcePath;
                    processViewModel.PastelInfo.VatFile = vsourcePath;
                    processViewModel.PastelInfo.Date = DateTime.Now;
                }
                ModelState["PastelInfo.WeighBridgeInfo"].Errors.Clear();
                ModelState["Sale.Customer.CompanyRegNumber"].Errors.Clear();
                if (ModelState.IsValid)
                {
                    _unit.PastelInfos.Add(processViewModel.PastelInfo);
                    _unit.Sale.Update(sale);
                    _unit.SaveChanges();

                    return RedirectToAction("UnProcessed");
                }
            }
            return View(processViewModel);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WeighBridgeInfo weighBridgeInfo = _unit.WeighBridgeInfos.GetAll()
                .FirstOrDefault(x => x.Id == id);

            if (weighBridgeInfo == null)
            {
                return HttpNotFound();
            }
            return View(weighBridgeInfo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Container container = _unit.Containers.GetAll()
                .Include(d => d.Booking).Include(w => w.WeighBridgeInfo)
                .FirstOrDefault(x => x.WeighBridgeInfo.Id == id);

            Sale sale = _unit.Sale.GetAll()
                .Include(w => w.WeighBridgeInfo)
                .FirstOrDefault(x => x.WeighBridgeInfo.Id == id);

            if (container != null)
            {
                container.Status = Statuses.Deleted;
                _unit.Containers.Update(container);
                _unit.SaveChanges();
            }
            if (sale != null)
            {
                _unit.Sale.Delete(sale);
                _unit.SaveChanges();
            }
            return RedirectToAction("UnProcessed");
        }

        public ActionResult LoadingSheet(int id)
        {
            Container container = _unit.Containers.GetAll()
                .Include(a => a.Booking)
                .Include(x => x.WeighBridgeInfo)
                .FirstOrDefault(x => x.Id == id);

            return ViewPdf(string.Empty, "LoadingSheet", container);
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
    }
}