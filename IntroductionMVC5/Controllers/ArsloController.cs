using IntroductionMVC5.Data;
using IntroductionMVC5.Models.ArsloTrading;
using IntroductionMVC5.Web.ViewModel;
using RustiviaSolutions.PDFGenerator;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace IntroductionMVC5.Web.Controllers
{
    public class ArsloController : Controller
    {
        private readonly ApplicationUnit _unit = new ApplicationUnit();
        private ArsloInvoiceGenerator arsloInvoiceGenerator = new ArsloInvoiceGenerator();
        // GET: Arslo
        public ActionResult Index()
        {
            ArsloViewModel arsloViewModel = new ArsloViewModel();
            arsloViewModel.Customers = GetAllCustomers();
            arsloViewModel.Profomas = GetAllProfomas();
            //arsloViewModel.Invoices = GetAllInvoices();

            return View(arsloViewModel);
        }

        #region Customers
        public ActionResult Customers()
        {
            List<ArsloCustomer> customers = GetAllCustomers();
            return View(customers);
        }

        public ActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCustomer(ArsloCustomer arsloCustomer)
        {
            _unit.ArsloCustomers.Add(arsloCustomer);
            _unit.SaveChanges();
            return RedirectToAction("Index");
        }

        private List<ArsloCustomer> GetAllCustomers()
        {
            List<ArsloCustomer> customers = _unit.ArsloCustomers.GetAll()
                .Include(iv => iv.Profomas)
                .OrderBy(s => s.CustomerName).ToList();
            return customers;
        }
        #endregion

        #region Invoices

        public ActionResult InvoiceDetails(int id)
        {
            var invoice = _unit.ArsloInvoices.GetAll()
                .Include(it => it.InvoiceItems).FirstOrDefault(iv => iv.Id == id);
            return View(invoice);
        }

        public ActionResult Invoices()
        {
            List<ArsloInvoice> profomas = GetAllInvoices();
            return View(profomas);
        }

        public ActionResult GenerateInvoice(int id)
        {
            ArsloProfoma profoma = _unit.ArsloProfomas.GetAll()
                .Include(pi=>pi.ProfomaItems)
                .FirstOrDefault(p => p.Id == id);

            //var stands =
            // profomas
            //.Select(s => new
            //{
            //    Id = s.Id,
            //    Description = string.Format("{0} - {1}", s.ProfomaNumber, s.Customer.CustomerName)
            //})
            // .ToList();

          

            List<ArsloInvoice> invoices = GetAllInvoices();
            string invoiceNumber = string.Format("INV-Arslo-{0}-{1}", DateTime.Now.Year, invoices.Count + 1);

            ViewBag.ProfomaItems = new SelectList(profoma.ProfomaItems, "Id", "Description");

            var newInvoice = new ArsloInvoice
            {
                Reference = invoiceNumber,
                Date = DateTime.Now,
                InvoiceItems = new List<ArsloInvoiceItem>(),
                Profoma = profoma
            };

            return View(newInvoice);
        }

        private List<ArsloInvoice> GetProfomaInvoices(int profomaId)
        {
            var paidInvoices = _unit.ArsloInvoices.GetAll()
                .Include(it => it.InvoiceItems).Include(pr => pr.Profoma)
                .Where(dd => dd.Profoma.Id == profomaId).ToList();

            return paidInvoices;
        }

        public string GetProfomaItems(int profomaId)
        {
            var profomas = _unit.ArsloProfomas.GetAll()
                 .Include(iv => iv.ProfomaItems).
                FirstOrDefault(p => p.Id == profomaId);

            //Get Paid invoice for profoma
            var paidInvoices = GetProfomaInvoices(profomaId);

            if (profomas.ProfomaItems != null && paidInvoices != null)
            {
                for (int i = 0; i < profomas.ProfomaItems.Count; i++)
                {
                    for (int j = 0; j < paidInvoices.Count; j++)
                    {
                        for (int k = 0; k < paidInvoices[j].InvoiceItems.Count; k++)
                        {
                            string invoiceItemDesc = paidInvoices[j].InvoiceItems[k].Description.Split('-')[0];
                            if (profomas.ProfomaItems[i].Description == invoiceItemDesc.Trim())
                            {
                                profomas.ProfomaItems[i].Quantity = profomas.ProfomaItems[i].Quantity - paidInvoices[j].InvoiceItems[k].Quantity;
                                profomas.ProfomaItems[i].TotalPrice = profomas.ProfomaItems[i].Quantity * profomas.ProfomaItems[i].Price;
                            }
                        }
                    }
                }
            }
            profomas.Invoices = null;

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(profomas);

            return json;
        }

        public ActionResult CustomerDetails(int customerId)
        {
            var customer = _unit.ArsloCustomers.GetAll().FirstOrDefault(pr => pr.Id == customerId);
            return View(customer);
        }

        public ActionResult EditCustomerDetails(int customerId)
        {
            var customer = _unit.ArsloCustomers.GetAll().FirstOrDefault(pr => pr.Id == customerId);
            return View(customer);
        }

        [HttpPost]
        public ActionResult EditCustomerDetails(ArsloCustomer customer)
        {
            var dbCustomer = _unit.ArsloCustomers.GetAll().FirstOrDefault(pr => pr.Id == customer.Id);

            dbCustomer.CustomerName = customer.CustomerName;
            dbCustomer.TellNumber = customer.TellNumber;
            dbCustomer.Address = customer.Address;

            _unit.ArsloCustomers.Update(dbCustomer);
            _unit.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult GenerateInvoice(ArsloInvoice arsloInvoice, FormCollection collection)
        {
            var count = collection["itemCount"].Split(',')[0];
            int itemCount = count != string.Empty ? Convert.ToInt32(count) : 0;
            List<ArsloInvoiceItem> items = new List<ArsloInvoiceItem>();
            decimal total = 0;
            for (int i = 0; i < itemCount; i++)
            {
                if (collection["description" + i] != string.Empty
                    && collection["quantity" + i] != string.Empty
                    && collection["unitPrice" + i] != string.Empty
                    && collection["unitTotalPrice" + i] != string.Empty)
                {
                    var desc = collection["desc" + i];

                    var item = new ArsloInvoiceItem
                    {
                        Description = collection["description" + i] + " - " + desc,
                        Quantity = Convert.ToDecimal(collection["quantity" + i]),
                        Price = Convert.ToDecimal(collection["unitPrice" + i]),
                        TotalPrice = Convert.ToDecimal(collection["unitTotalPrice" + i])
                    };
                    total = total + Convert.ToDecimal(collection["unitTotalPrice" + i]);
                    items.Add(item);
                }
            }
            ArsloProfoma arsloProfoma = _unit.ArsloProfomas.GetAll().Include(cu => cu.Customer)
                .FirstOrDefault(pr => pr.Id == arsloInvoice.Profoma.Id);

            arsloInvoice.Customer = arsloProfoma.Customer;
            arsloInvoice.Profoma = arsloProfoma;
            arsloInvoice.InvoiceItems = items;
            arsloInvoice.TotalPrice = total;
            string invoiceLocation = arsloInvoiceGenerator.GenerateInvoice(arsloInvoice);
            arsloInvoice.InvoiceLocation = invoiceLocation;
            _unit.ArsloInvoices.Add(arsloInvoice);
            _unit.SaveChanges();

            return RedirectToAction("ProfomaDetails", new { id = arsloProfoma.Id });
        }
        private List<ArsloInvoice> GetAllInvoices()
        {
            List<ArsloInvoice> profomas = _unit.ArsloInvoices.GetAll()
                .Include(iv => iv.InvoiceItems)
                .OrderByDescending(s => s.Date).ToList();
            return profomas;
        }
        #endregion

        #region Profomas
        public ActionResult AddProfoma()
        {
            var customers = GetAllCustomers();
            ViewBag.Customers = new SelectList(customers, "Id", "CustomerName");
            ViewBag.Status = new SelectList(new List<string> { "Paid", "Part Payment", "Pending Payment" });
            var profomas = GetAllProfomas();

            var ucr = string.Format("{0}ZA21366338-Arslo-{1}", DateTime.Now.Year.ToString().Substring(2, 2), profomas.Count + 1);

            var profoma = string.Format("Arslo-{0}-{1}", DateTime.Now.Year.ToString().Substring(2, 2), profomas.Count + 1);

            ArsloProfoma newProfoma = new ArsloProfoma
            {
                ProfomaNumber = profoma,
                Date = DateTime.Now,
                UCRNumber = ucr,
                ProfomaItems = new List<ArsloProfomaItem>()
            };

            return View(newProfoma);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProfoma(ArsloProfoma profoma, FormCollection collection)
        {
            ArsloCustomer arsloCustomer = _unit.ArsloCustomers.GetAll()
                                            .FirstOrDefault(cu => cu.Id == profoma.Customer.Id);

            int itemCount = collection["itemCount"] != string.Empty ? Convert.ToInt32(collection["itemCount"]) : 0;
            List<ArsloProfomaItem> items = new List<ArsloProfomaItem>();
            for (int i = 0; i < itemCount; i++)
            {
                if (collection["description" + i] != string.Empty
                    && collection["quantity" + i] != string.Empty
                    && collection["unitPrice" + i] != string.Empty
                    && collection["unitTotalPrice" + i] != string.Empty)
                {
                    var item = new ArsloProfomaItem
                    {
                        Description = collection["description" + i],
                        Quantity = Convert.ToDecimal(collection["quantity" + i]),
                        Price = Convert.ToDecimal(collection["unitPrice" + i]),
                        TotalPrice = Convert.ToDecimal(collection["unitTotalPrice" + i])
                    };
                    items.Add(item);
                    profoma.Amount = profoma.Amount.HasValue ? profoma.Amount.Value + item.TotalPrice : item.TotalPrice;
                }
            }
            if (itemCount > 0)
            {
                profoma.ProfomaItems = items;
                string profomaLocation = arsloInvoiceGenerator.GenerateProfoma(profoma, arsloCustomer);

                profoma.Location = profomaLocation;
                profoma.Customer = arsloCustomer;
                _unit.ArsloProfomas.Add(profoma);
                _unit.SaveChanges();
            }


            return Redirect("Index");
        }
        private List<ArsloProfoma> GetAllProfomas()
        {
            List<ArsloProfoma> profomas = _unit.ArsloProfomas.GetAll()
                .Include(iv => iv.Invoices)
                .Include(c => c.Customer)
                .Include(pi => pi.ProfomaItems)
                .Include(dd => dd.ProfomaDrawDowns)
                .OrderByDescending(s => s.Date).ToList();
            return profomas;
        }
        public ActionResult Profomas()
        {
            List<ArsloProfoma> profomas = GetAllProfomas();
            return View(profomas);

        }

        public ActionResult ProfomaDetails(int id)
        {
            ViewBag.Status = new SelectList(new List<string> { "", "Paid", "Part Payment", "Pending Payment" });

            var profoma = _unit.ArsloProfomas.GetAll()
                .Include(dr => dr.ProfomaDrawDowns)
                .Include(dr => dr.ProfomaItems)
                .Include(iv => iv.Invoices)
                .Include("Invoices.InvoiceItems")
                .FirstOrDefault(pr => pr.Id == id);

            return View(profoma);
        }

        public ActionResult EditProfoma(int id)
        {
            var profoma = _unit.ArsloProfomas.GetAll()
                        .Include(p => p.ProfomaItems)
                        .FirstOrDefault(pr => pr.Id == id);
            ViewBag.Status = new SelectList(new List<string> { "", "Paid", "Part Payment", "Pending Payment" });

            return View(profoma);
        }

        [HttpPost]
        public ActionResult EditProfoma(ArsloProfoma profoma, FormCollection collection)
        {
            var dbProfoma = _unit.ArsloProfomas.GetAll()
                .Include(c => c.Customer)
                .Include(pi => pi.ProfomaItems)
                .Include(dw => dw.ProfomaDrawDowns)
                .FirstOrDefault(pr => pr.Id == profoma.Id);

            dbProfoma.Status = profoma.Status;
            int itemCount = dbProfoma.ProfomaItems != null ? dbProfoma.ProfomaItems.Count : 0;
            dbProfoma.Amount = 0;

            for (int i = 0; i < itemCount; i++)
            {
                dbProfoma.Amount = 0;
                if (collection["description" + i] != null
                    && collection["quantity" + i] != null
                    && collection["unitPrice" + i] != null
                    && collection["unitTotalPrice" + i] != null
                    && collection["id" + i] != null)
                {
                    int id = Convert.ToInt32(collection["id" + i]);
                    ArsloProfomaItem pi = dbProfoma.ProfomaItems.FirstOrDefault(p => p.Id == id);

                    pi.Description = collection["description" + i];
                    pi.Price = Convert.ToDecimal(collection["unitPrice" + i]);
                    pi.Quantity = Convert.ToDecimal(collection["quantity" + i]);
                    pi.TotalPrice = Convert.ToDecimal(collection["unitTotalPrice" + i]);

                    dbProfoma.Amount = dbProfoma.Amount.HasValue ? dbProfoma.Amount.Value + pi.TotalPrice : pi.TotalPrice;
                }
            }
            if (System.IO.File.Exists(dbProfoma.Location))
            {
                System.IO.File.Delete(dbProfoma.Location);
            }

            string profomaLocation = arsloInvoiceGenerator.GenerateProfoma(dbProfoma, dbProfoma.Customer);
            dbProfoma.Location = profomaLocation;

            _unit.ArsloProfomas.Update(dbProfoma);
            _unit.SaveChanges();
            return RedirectToAction("ProfomaDetails", new { id = profoma.Id });
        }

        public ActionResult DrawDownProfoma(int id)
        {
            var profoma = _unit.ArsloProfomas.GetAll()
                .Include(pi => pi.ProfomaItems)
                .Include(dw => dw.ProfomaDrawDowns)
                .FirstOrDefault(pr => pr.Id == id);

            ViewBag.Status = new SelectList(new List<string> { "", "Paid", "Part Payment", "Pending Payment" });

            profoma.Amount = profoma.Amount - profoma.ProfomaDrawDowns.Sum(a => a.Amount);

            return View(profoma);
        }

        [HttpPost]
        public ActionResult DrawDownProfoma(ArsloProfoma profoma, FormCollection collection)
        {
            ArsloProfoma dbProfoma = _unit.ArsloProfomas
                                    .GetAll().Include(c => c.Customer)
                                    .Include(pd => pd.ProfomaDrawDowns)
                                    .Include(iv => iv.Invoices)
                                    .FirstOrDefault(pr => pr.Id == profoma.Id);

            if (collection["reference"] != string.Empty &&
                collection["drawdownAmount"] != string.Empty)
            {
                var drawDown = new ArsloProfomaDrawDown
                {
                    Date = DateTime.Now,
                    Amount = Convert.ToDecimal(collection["drawdownAmount"]),
                    Reference = collection["reference"]
                };

                dbProfoma.ProfomaDrawDowns.Add(drawDown);
                _unit.ArsloProfomas.Update(dbProfoma);
                _unit.SaveChanges();
                return RedirectToAction("ProfomaDetails", new { id = profoma.Id });
            }
            return DrawDownProfoma(profoma.Id);
        }

        private string MoveIdFile(ArsloProfoma profoma)
        {
            string idSourcePath = "Location";
            string profomasLocation = ConfigurationManager.AppSettings["Profoma"];
            if (!Directory.Exists(Server.MapPath("~/App_Data" + profomasLocation)))
            {
                Directory.CreateDirectory(Server.MapPath("~/App_Data" + profomasLocation));
            }
            if (Request.Files.Count > 0 && Request.Files["ProfomaLocation"] != null
                && !string.IsNullOrEmpty(Request.Files["ProfomaLocation"].FileName))
            {
                string idFileName = Request.Files["ProfomaLocation"].FileName;
                string extension = Path.GetExtension(idFileName);
                idSourcePath = Path.Combine(Server.MapPath("~/App_Data" + profomasLocation),
                    string.Format("{0}_P{1}", profoma.ProfomaNumber, extension));
                Request.Files["ProfomaLocation"].SaveAs(idSourcePath);
                System.IO.File.AppendAllText(@"c:\temp\errors.txt",
                    string.Format("File Location:  {0}", Server.MapPath("~/App_Data" + profomasLocation)));
                ModelState.SetModelValue("IdLocation",
                    new ValueProviderResult(new List<string>
                    {
                        idSourcePath
                    }, idSourcePath, CultureInfo.CurrentCulture));

                ModelState["IdLocation"].Errors.Clear();
            }
            return idSourcePath;
        }
        #endregion

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

        public ViewResultBase Search(string search)
        {
            List<ArsloCustomer> customers =
                _unit.ArsloCustomers.GetAll().OrderBy(s => s.CustomerName).ToList();

            //alfabet, first letter
            if (!string.IsNullOrEmpty(search))
            {
                customers = customers.OrderBy(s => s.CustomerName)
                    .Where(s => s.CustomerName.ToUpper().Contains(search.ToUpper())).ToList();
            }

            ArsloViewModel arsloViewModel = new ArsloViewModel();
            arsloViewModel.Customers = customers;
            arsloViewModel.Profomas = GetAllProfomas();
            arsloViewModel.Invoices = GetAllInvoices();

            return View("Index", arsloViewModel);
        }
    }
}