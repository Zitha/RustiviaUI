using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IntroductionMVC5.Data;
using IntroductionMVC5.Models;
using IntroductionMVC5.Models.Integrator;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IntroductionMVC5.Web.Controllers
{
    [Authorize]
    public class TruckController : Controller
    {
        //
        // GET: /Truck/
        private readonly ApplicationUnit _unit = new ApplicationUnit();
        private List<Truck> _trucks = new List<Truck>();

        public TruckController()
        {
            _trucks = GetAllTrucks();
        }

        private List<Truck> GetAllTrucks()
        {
            List<Truck> trucks = _unit.Truck.GetAll().ToList();
            return trucks;
        }

        public ActionResult Index()
        {
            var settings =
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

            //string jsonTrucks = JsonConvert.SerializeObject(_trucks, settings);
            //ViewBag.TruckList = jsonTrucks;

            return View(_trucks);
        }

        [HttpGet]
        public ActionResult Create()
        {
            List<SupplierInfo> suppliers = _unit.SupplierInfo.GetAll().OrderBy(s => s.SupplierName).ToList();
            ViewBag.Id = new SelectList(suppliers, "Id", "SupplierName");

            var supplier = new Truck
            {
                Image = "Truck.png"
            };
            return View(supplier);
        }

        [HttpPost]
        public ActionResult Create(Truck truck)
        {
            List<SupplierInfo> suppliers = _unit.SupplierInfo.GetAll()
                .ToList();
            if (ModelState.IsValid)
            {
                truck.SupplierInfo = suppliers.FirstOrDefault(s => s.Id == truck.Id);

                _unit.Truck.Add(truck);
                _unit.SaveChanges();
                _trucks = GetAllTrucks();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}