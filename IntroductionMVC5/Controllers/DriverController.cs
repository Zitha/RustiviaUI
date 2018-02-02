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
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IntroductionMVC5.Web.Controllers
{
    [Authorize]
    public class DriverController : PdfViewController
    {
        private readonly ApplicationUnit _unit = new ApplicationUnit();
        private List<Driver> _drivers = new List<Driver>();
        private readonly int _defaultPageSize =
          Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPaginationSize"]);

        public DriverController()
        {
            _drivers = GetAllDrivers();
        }

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult Index(int? page)
        {
            var settings =
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

            string jsonDrivers = JsonConvert.SerializeObject(_drivers, settings);

            ViewBag.DriverList = jsonDrivers;

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            IPagedList<Driver> providersListPaged = _drivers.ToPagedList(currentPageIndex,
                _defaultPageSize);

            return View(providersListPaged);
        }

        private List<Driver> GetAllDrivers()
        {
            List<Driver> drivers = _unit.Drivers.GetAll()
                .OrderBy(s => s.Firstname).
                ToList();
            return drivers;
        }

        public ActionResult Details(int id)
        {
            Driver driver = _unit.Drivers.GetAll().FirstOrDefault(a => a.Id == id);

            var settings =
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            string jsonDriver = JsonConvert.SerializeObject(driver, settings);

            var d = JsonConvert.DeserializeObject(jsonDriver, typeof(Driver));

            List<Purchase> purchases = _unit.Purchase
                .GetAll().Include(w => w.WeighBridgeInfo)
                .Where(w => w.Driver.Id == id && w.Status == Statuses.Processed).ToList();
            var pastelInfos = new List<PastelInfo>();
            if (purchases.Count > 0)
            {
                pastelInfos = (from pastelInfo in _unit.PastelInfos.GetAll().Include(w => w.WeighBridgeInfo).ToList()
                               join w in purchases on pastelInfo.WeighBridgeInfo.Id equals w.WeighBridgeInfo.Id
                               select pastelInfo).ToList();
            }

            var driverViewModel = new DriverDetailsViewModel
            {
                Purchases = purchases,
                PastelInfos = pastelInfos,
                Driver = driver
            };

            return View(driverViewModel);
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

        [HttpGet]
        public ActionResult Create()
        {
            List<SupplierInfo> suppliers = _unit.SupplierInfo.GetAll().Where(i => !i.IsBlocked)
                .OrderBy(s => s.SupplierName).ToList();
            var driver = new Driver { ImageName = "Men.png" };

            ViewBag.Id = new SelectList(suppliers, "Id", "SupplierName");
            ViewBag.Gender = new SelectList(new List<string> { "Male", "Female" });
            ViewBag.Notification = string.Empty;
            return View(driver);
        }

        [HttpGet]
        public ActionResult Copy(int id)
        {
            Driver driver = _unit.Drivers.GetAll().FirstOrDefault(a => a.Id == id);

            List<SupplierInfo> suppliers = _unit.SupplierInfo.GetAll()
               .OrderBy(s => s.SupplierName).ToList();

            ViewBag.Id = new SelectList(suppliers, "Id", "SupplierName");
            return View(driver);
        }

        [HttpPost]
        public ActionResult Copy(Driver driver)
        {
            List<SupplierInfo> suppliers = _unit.SupplierInfo.GetAll()
               .OrderBy(s => s.SupplierName).ToList();
            if (ModelState.IsValid)
            {
                BlockDriver(driver.IdNumber);

                driver.SupplierInfo = suppliers.FirstOrDefault(s => s.Id == driver.Id);

                _unit.Drivers.Add(driver);
                _unit.SaveChanges();
                _drivers = GetAllDrivers();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(suppliers, "Id", "SupplierName");
            return View(driver);
        }

        private void BlockDriver(string idNumber)
        {
            var driver = _unit.Drivers.GetAll()
                .FirstOrDefault(i => i.IdNumber == idNumber);

            if (driver != null)
            {
                driver.IsBlocked = true;
                _unit.Drivers.Update(driver);
                _unit.SaveChanges();
            }
        }

        [HttpPost]
        public ActionResult Create(Driver driver)
        {
            List<SupplierInfo> suppliers = _unit.SupplierInfo.GetAll()
                   .ToList();


            if (!_drivers.Exists(d => d.IdNumber == driver.IdNumber && !d.IsBlocked))
            {
                if (ModelState.IsValid)
                {
                    //ID location Info
                    string idSourcePath = MoveIdFile(driver);

                    System.IO.File.AppendAllText(@"c:\temp\errors.txt", string.Format("ID Location:  {0}", idSourcePath));
                    Console.WriteLine();

                    driver.SupplierInfo = suppliers.FirstOrDefault(s => s.Id == driver.Id);
                    driver.IdLocation = idSourcePath ?? "Location";

                    _unit.Drivers.Add(driver);
                    _unit.SaveChanges();
                    _drivers = GetAllDrivers();
                    return RedirectToAction("Index");
                }
                ViewBag.Gender = new SelectList(new List<string> { "Male", "Female" });
                ViewBag.Id = new SelectList(suppliers, "Id", "SupplierName");
                return View();
            }

            ViewBag.Gender = new SelectList(new List<string> { "Male", "Female" });
            ViewBag.Id = new SelectList(suppliers, "Id", "SupplierName");
            ViewBag.Notification = "Driver with Id number Already Exists";
            ViewBag.DriverId = _unit.Drivers.GetAll().FirstOrDefault(i => i.IdNumber == driver.IdNumber).Id;
            return View();
        }

        private string MoveIdFile(Driver driver)
        {
            string idSourcePath = "Location";
            string driverIdsLocation = ConfigurationManager.AppSettings["DriverInfo"];
            if (!Directory.Exists(Server.MapPath("~/App_Data" + driverIdsLocation)))
            {
                Directory.CreateDirectory(Server.MapPath("~/App_Data" + driverIdsLocation));
            }
            if (Request.Files.Count > 0 && Request.Files["IdLocation"] != null
                && !string.IsNullOrEmpty(Request.Files["IdLocation"].FileName))
            {
                string idFileName = Request.Files["IdLocation"].FileName;
                string extension = Path.GetExtension(idFileName);
                idSourcePath = Path.Combine(Server.MapPath("~/App_Data" + driverIdsLocation),
                    string.Format("{0}_ID{1}", driver.IdNumber, extension));
                Request.Files["IdLocation"].SaveAs(idSourcePath);
                System.IO.File.AppendAllText(@"c:\temp\errors.txt",
                    string.Format("File Location:  {0}", Server.MapPath("~/App_Data" + driverIdsLocation)));
                ModelState.SetModelValue("IdLocation",
                    new ValueProviderResult(new List<string>
                    {
                        idSourcePath
                    }, idSourcePath, CultureInfo.CurrentCulture));

                ModelState["IdLocation"].Errors.Clear();
            }
            return idSourcePath;
        }

        public ActionResult Edit(int id)
        {
            Driver driver = _unit.Drivers.GetAll()
                .FirstOrDefault(x => x.Id == id);

            return View(driver);
        }

        [HttpPost]
        public ActionResult Edit(Driver driver)
        {
            Driver driverToUpdate = _unit.Drivers.GetAll()
                .FirstOrDefault(x => x.Id == driver.Id);

            string idSourcepath = string.Empty;
            if (Request.Files.Count > 0 && Request.Files["IdLocation"] != null
                && !string.IsNullOrEmpty(Request.Files["IdLocation"].FileName))
            {
                DeleteFile(driverToUpdate.IdLocation);
                idSourcepath = MoveIdFile(driver);
            }

            if (driverToUpdate != null)
            {
                driverToUpdate.Firstname = !string.IsNullOrEmpty(driver.Firstname)
                    ? driver.Firstname
                    : driverToUpdate.Firstname;
                driverToUpdate.Surname = !string.IsNullOrEmpty(driver.Surname)
                    ? driver.Surname
                    : driverToUpdate.Surname;
                driverToUpdate.IdLocation = string.IsNullOrEmpty(idSourcepath)
                    ? driverToUpdate.IdLocation
                    : idSourcepath;
                driverToUpdate.IdNumber = !string.IsNullOrEmpty(driver.IdNumber)
                    ? driver.IdNumber
                    : driverToUpdate.IdNumber;
            }

            _unit.Drivers.Update(driverToUpdate);
            _unit.SaveChanges();

            return RedirectToAction("Index");
        }

        private void DeleteFile(string idLocation)
        {
            if (System.IO.File.Exists(idLocation))
            {
                System.IO.File.Delete(idLocation);
            }
        }

        public ViewResultBase Search(string search)
        {
            const int currentPageIndex = 0;
            List<Driver> drivers =
                _unit.Drivers.GetAll().OrderBy(s => s.Firstname).ToList();

            if (!string.IsNullOrEmpty(search))
            {
                drivers = drivers.OrderBy(s => s.Firstname)
                    .Where(
                        s =>
                            s.Firstname.ToUpper().Contains(search.ToUpper()) ||
                            s.Surname.ToUpper().Contains(search.ToUpper())).ToList();
            }
            IPagedList<Driver> providersListPaged = drivers.ToPagedList(currentPageIndex,
               _defaultPageSize);

            if (Request.IsAjaxRequest())
                return PartialView("Index", providersListPaged);
            return View("Index", providersListPaged);
        }
    }
}