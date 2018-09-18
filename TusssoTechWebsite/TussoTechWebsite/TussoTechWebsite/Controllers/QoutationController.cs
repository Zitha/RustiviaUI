using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using TussoTechWebsite.Model;
using TussoTechWebsite.Models;
using TussoTechWebsite.PDFGenerator;

namespace TussoTechWebsite.Controllers
{
    public class QoutationController : Controller
    {
        private HelperFunction _helperFunction = new HelperFunction();
        private readonly QouteGenerator _generator = new QouteGenerator();

        public PartialViewResult QoutationView()
        {
            using (DataClass dataClass = new DataClass())
            {
                List<Qoutation> qoutes = dataClass.GetMoreQoute();
                List<OnceOffInvoice> onceOffInvoice = dataClass.GetOnceOffInvoice();

                return PartialView("_QoutationView", new InvoiceQoutation
                {
                    Invoices = onceOffInvoice,
                    Qoutations = qoutes
                });
            }
        }

        [HttpGet]
        public PartialViewResult CreateQoutation(int id = 0)
        {
            return PartialView("_CreateQoutation", new Qoutation
            {
                Customer_Id = id
            });
        }

        [HttpGet]
        public ActionResult EditQoutation(int id)
        {
            using (DataClass dataClass = new DataClass())
            {
                var status = new List<string> { Status.Unpaid.ToString(), Status.Paid.ToString() };

                ViewBag.Status = new SelectList(status);

                var selectedQoutation = dataClass.GetQoutation(id);

                return PartialView("_EditQoutation", selectedQoutation);
            }
        }

        [HttpPost]
        public ActionResult CreateQoutation(Qoutation qoutation, FormCollection collection, string email)
        {
            using (DataClass dataClass = new DataClass())
            {

                var items = new List<Item>();

                int itemCount = Convert.ToInt32(collection["itemCount"]);

                for (int i = 0; i < itemCount - 1; i++)
                {
                    var item = new Item
                    {
                        Description = collection["description" + i],
                        Quantity = Convert.ToInt32(collection["quantity" + i]),
                        UnitPrice = Convert.ToInt32(collection["unitPrice" + i])
                    };
                    items.Add(item);
                }

                if (qoutation.Customer_Id == 0)
                {
                    string customerName = collection["customerName"];
                    string customerAddress = collection["customerAddress"];

                    qoutation.CustomerName = customerName;
                    qoutation.Address = customerAddress;
                }
                else
                {
                    Customer customer = dataClass.GetCustomer(qoutation.Customer_Id);
                    qoutation.CustomerName = customer.Name;
                    qoutation.Address = customer.Address;
                }

                qoutation.Items = items;
                qoutation.DateSent = DateTime.Now;
                qoutation.QoutationNumber = string.Format("QUOTE-{0}", 1000 + dataClass.GetQouteCount());

                //Create PDF Documentcresta
                string attachment = _generator.CreateQoute(qoutation);

                qoutation.Location = attachment;
                //Send Email With Invoice.
                _helperFunction.SendQouteEmail(email, qoutation.Description, attachment);

                //Save Invoice In the Database
                dataClass.CreateQoutation(qoutation);

                return RedirectToAction("Index", "Employee", new
                {
                    page = "MoreFuncation"
                });
            }
        }

        public ActionResult GetFile(string fileName)
        {
            using (var webClient = new WebClient())
            {
                if (!System.IO.File.Exists(fileName))
                {
                    return HttpNotFound();
                }
                byte[] file = webClient.DownloadData(fileName);
                string filetype = Path.GetExtension(fileName);
                if (filetype != null &&
                    filetype.ToUpper() != ".PDF")
                {
                    var dd = File(file, filetype, Path.GetFileName(fileName));
                    return File(file, filetype, Path.GetFileName(fileName));
                }
                var vv = File(file, MediaTypeNames.Application.Pdf);
                return File(file, MediaTypeNames.Application.Pdf);
            }
        }
    }
}