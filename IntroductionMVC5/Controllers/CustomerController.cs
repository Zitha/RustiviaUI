    using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using IntroductionMVC5.Data;
using IntroductionMVC5.Models;
using IntroductionMVC5.Models.Integrator;
using IntroductionMVC5.Web.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IntroductionMVC5.Web.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ApplicationUnit _unit = new ApplicationUnit();
        private List<Customer> _customers = new List<Customer>();

        public CustomerController()
        {
            _customers = GetAllCustomers();
        }

        private static SelectList Months
        {
            get
            {
                return new SelectList(new List<string>
                {
                    "January",
                    "February",
                    "March",
                    "April",
                    "May",
                    "June",
                    "July",
                    "August",
                    "September",
                    "October",
                    "November",
                    "December"
                });
            }
        }


        private static SelectList Years
        {
            get
            {
                return new SelectList(new List<string>
                {
                    "2014",
                    "2015",
                    "2016",
                    "2017",
                    "2018",
                    "2019",
                    "2020",
                    "2021",
                    "2022",
                    "2023",
                    "2024",
                    "2025"
                });
            }
        }
        private List<Customer> GetAllCustomers()
        {
            List<Customer> customers = _unit.Customers.GetAll()
                .Include(b => b.Bookings)
                .OrderBy(s => s.CustomerName).ToList();
            return customers;
        }

        [HttpGet]
        public ActionResult Index(int? month)
        {
            var settings =
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

            string jsonCustomers = JsonConvert.SerializeObject(_customers, settings);
            ViewBag.CustomerList = jsonCustomers;
            return View(_customers);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var customer = new Customer
            {
                Logo = "Delivery.png"
            };
            return View(customer);
        }

        [HttpPost]
        public ActionResult Create(Customer clientInfo)
        {
            if (ModelState.IsValid)
            {
                _unit.Customers.Add(clientInfo);
                _unit.SaveChanges();
                _customers = GetAllCustomers();
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Details(int id)
        {
            Customer customer = _unit.Customers.GetAll().
                Include(x => x.Bookings).FirstOrDefault(a => a.Id == id);

            List<Booking> customerbooking = _unit.Bookings.GetAll().
                Include(x => x.Containers).Where(x => x.Customer.Id == id).ToList();
            customer.Bookings = customerbooking;
            return View(customer);
        }

        public ActionResult AddBooking(int id)
        {
            Customer customer = _unit.Customers.GetAll().
                Include(x => x.Bookings).FirstOrDefault(a => a.Id == id);

            List<Transporter> transporter = _unit.Transporter.GetAll()
                .OrderBy(s => s.Name).ToList();

            ViewBag.Id = new SelectList(transporter, "Id", "Name");
            var booking = new Booking
            {
                Customer = customer,
                DateIn = DateTime.Now
            };

            return View(booking);
        }

        [HttpPost]
        public ActionResult AddBooking(Booking booking)
        {
            Customer customer = _unit.Customers.GetAll().
                Include(x => x.Bookings).FirstOrDefault(a => a.Id == booking.Customer.Id);
            List<Transporter> transporter = _unit.Transporter.GetAll()
                .OrderBy(s => s.Name).ToList();
            booking.Customer = customer;

            ICollection<string> keys = ModelState.Keys;
            foreach (string key in keys)
            {
                if (key.Contains("Transporter"))
                {
                    ModelState[key].Errors.Clear();
                }
            }

            if (ModelState.IsValid)
            {
                booking.Status = Statuses.Unprocessed;
                booking.Transporter = transporter.FirstOrDefault(w => w.Id == booking.Transporter.Id);
                _unit.Bookings.Add(booking);
                _unit.SaveChanges();
                _customers = GetAllCustomers();
                return RedirectToAction("Details", new { id = customer.Id });
            }
            return View(booking);
        }

        public ActionResult AddContainer(string id)
        {
            Booking booking = _unit.Bookings.GetAll().Include(t => t.Transporter)
                .FirstOrDefault(x => x.Reference == id);

            var container = new Container
            {
                DateIn = DateTime.Now,
                Booking = booking
            };
            return View(container);
        }

        [HttpPost]
        public ActionResult AddContainer(Container container)
        {
            Booking booking = _unit.Bookings.GetAll().Include(a => a.Customer)
                .FirstOrDefault(x => x.Reference == container.Booking.Reference);

            ICollection<string> keys = ModelState.Keys;
            foreach (string key in keys)
            {
                if (key.Contains("Id"))
                {
                    ModelState[key].Errors.Clear();
                }
            }

            if (ModelState.IsValid)
            {
                container.Booking = booking;
                container.Status = Statuses.Waiting;
                _unit.Containers.Add(container);
                _unit.SaveChanges();
                _customers = GetAllCustomers();
                return RedirectToAction("Details", new { id = booking.Customer.Id });
            }

            return View(new Container
            {
                Booking = booking,
                DateIn = DateTime.Now
            });
        }

        public ActionResult BookingContainers(string referenceNumber)
        {
            Booking booking = _unit.Bookings.GetAll().Include(a => a.Containers)
                .FirstOrDefault(x => x.Reference == referenceNumber);

            return View(booking);
        }

        public ActionResult ContainerReport()
        {
            int selectedMonth = DateTime.Now.Month;
            ViewBag.Month = Months;
            ViewBag.Years = Years;
            ViewBag.SelectedMonth = Months.ToList()[selectedMonth - 1].Text;
            ViewBag.SelectedYear = DateTime.Now.Year.ToString();

            IList<Container> containers = _unit.Customers.GetAll()
                .Include(b => b.Bookings.Select(c => c.Containers))
                .SelectMany(
                    x =>
                        x.Bookings
                            .SelectMany(
                                cnt =>
                                    cnt.Containers.Where(
                                        t => t.Status == Statuses.Processed && t.DateIn.Month == selectedMonth))
                            .ToList()).OrderByDescending(d => d.WeighBridgeInfo.DateOut)
                .ToList();


            List<PastelInfo> pastelInfos =
                (from pastelInfo in _unit.PastelInfos.GetAll().Include(i => i.WeighBridgeInfo)
                    .OrderByDescending(i => i.WeighBridgeInfo.DateOut).ToList()
                 join w in containers on pastelInfo.WeighBridgeInfo.Id equals w.WeighBridgeInfo.Id
                 select pastelInfo).ToList();

            ViewBag.PastelInfos = pastelInfos;
            return View(containers);
        }

        [HttpPost]
        public ActionResult ContainerReport(int year, string month)
        {
            ViewBag.Month = Months;
            ViewBag.Years = Years;
            int selectedMonth = GetSelectedMonth(month);
            ViewBag.SelectedMonth = month;
            ViewBag.SelectedYear = year;


            IList<Container> containers = _unit.Customers.GetAll()
                .Include(b => b.Bookings.Select(c => c.Containers))
                .SelectMany(
                    x =>
                        x.Bookings
                            .SelectMany(
                                cnt =>
                                    cnt.Containers.Where(
                                        t => t.Status == Statuses.Processed && (t.DateIn.Year == year && t.DateIn.Month == selectedMonth)))
                            .ToList()).OrderByDescending(d => d.WeighBridgeInfo.DateOut)
                .ToList();


            List<PastelInfo> pastelInfos =
                (from pastelInfo in _unit.PastelInfos.GetAll().Include(i => i.WeighBridgeInfo)
                    .OrderByDescending(i => i.WeighBridgeInfo.DateOut).ToList()
                 join w in containers on pastelInfo.WeighBridgeInfo.Id equals w.WeighBridgeInfo.Id
                 select pastelInfo).ToList();

            ViewBag.PastelInfos = pastelInfos;
            return View(containers);
        }

        private int GetSelectedMonth(string month)
        {
            int selectedMonth = 0;
            switch (month)
            {
                case "January":
                    selectedMonth = 1;
                    break;
                case "February":
                    selectedMonth = 2;
                    break;
                case "March":
                    selectedMonth = 3;
                    break;
                case "April":
                    selectedMonth = 4;
                    break;
                case "May":
                    selectedMonth = 5;
                    break;
                case "June":
                    selectedMonth = 6;
                    break;
                case "July":
                    selectedMonth = 7;
                    break;
                case "August":
                    selectedMonth = 8;
                    break;
                case "September":
                    selectedMonth = 9;
                    break;
                case "October":
                    selectedMonth = 10;
                    break;
                case "November":
                    selectedMonth = 11;
                    break;
                case "December":
                    selectedMonth = 12;
                    break;
            }
            return selectedMonth;
        }

        public ActionResult EditContainer(int id)
        {
            Container container = _unit.Containers.GetAll().Include(a => a.Booking)
                .FirstOrDefault(x => x.Id == id);

            return View("EditBooking", container);
        }

        public ActionResult EditCompletedContainer(int id)
        {
            Container container = _unit.Containers.GetAll().Include(a => a.Booking)
                .FirstOrDefault(x => x.Id == id);

            return View("EditCompletedContainer", container);
        }

        [HttpPost]
        public ActionResult EditCompletedContainer(Container container)
        {
            Container containerToUpdate = _unit.Containers.GetAll().Include(a => a.Booking)
                .FirstOrDefault(x => x.Id == container.Id);

            if (containerToUpdate != null)
            {
                containerToUpdate.Invoice1 = container.Invoice1;
                containerToUpdate.Invoice2 = container.Invoice2;

                if (!string.IsNullOrEmpty(containerToUpdate.Invoice1) &&
                    !string.IsNullOrEmpty(containerToUpdate.Invoice2))
                {
                    containerToUpdate.Paid = true;
                }

                _unit.Containers.Update(containerToUpdate);
                _unit.SaveChanges();

                return RedirectToAction("ContainerReport");
            }
            return View(container);
        }

        [HttpPost]
        public ActionResult EditContainer(Container cntainer)
        {
            Container container = _unit.Containers.GetAll().Include(b => b.Booking.Customer)
                .FirstOrDefault(x => x.Id == cntainer.Id);

            container.TareWeight = cntainer.TareWeight;
            container.ContainerNumber = cntainer.ContainerNumber;
            container.Sealnumber = cntainer.Sealnumber;
            container.Product = cntainer.Product;

            _unit.Containers.Update(container);
            _unit.SaveChanges();

            string url = Url.Action("BookingContainers", "Customer") + "?referenceNumber=" + container.Booking.Reference;
            return Redirect(url);
        }

        public ViewResultBase Search(string search)
        {
            List<Customer> customers =
                _unit.Customers.GetAll().OrderBy(s => s.CustomerName).ToList();

            //alfabet, first letter
            if (!string.IsNullOrEmpty(search))
            {
                customers = customers.OrderBy(s => s.CustomerName)
                    .Where(s => s.CustomerName.ToUpper().Contains(search.ToUpper())).ToList();
            }

            if (Request.IsAjaxRequest())
                return PartialView("Index", customers);
            return View("Index", customers);
        }
    }
}