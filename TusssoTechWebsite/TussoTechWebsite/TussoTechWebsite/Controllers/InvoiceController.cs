using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TussoTechWebsite.Model;
using TussoTechWebsite.Models;
using TussoTechWebsite.PDFGenerator;

namespace TussoTechWebsite.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly InvoiceGenerator _generator = new InvoiceGenerator();
        private HelperFunction _helperFunction = new HelperFunction();

        [HttpGet]
        public ActionResult EditInvoice(int id)
        {
            using (DataClass dataClass = new DataClass())
            {
                var status = new List<string> { Status.Unpaid.ToString(), Status.Paid.ToString() };

                ViewBag.Status = new SelectList(status);

                var selectedInvocie = dataClass.GetInvoice(id);

                return PartialView("_EditInvoice", selectedInvocie);
            }
        }

        [HttpGet]
        public PartialViewResult CreateInvoice()
        {
            return PartialView("_CreateInvoice", new Invoice());
        }

        [HttpGet]
        public PartialViewResult CreateOnceOffInvoice()
        {
            return PartialView("_CreateOnceOffInvoice", new OnceOffInvoice());
        }

        [HttpPost]
        public ActionResult EditInvoice(Invoice invoice)
        {
            using (DataClass dataClass = new DataClass())
            {
                if (ModelState.IsValid)
                {
                    Invoice invoiceToUpdate = dataClass.GetInvoice(invoice.Id);
                    invoiceToUpdate.Status = invoice.Status;
                    invoiceToUpdate.Description = invoice.Description;
                    invoiceToUpdate.Total = invoice.Total;

                    if (Request.Files.Count > 0 && Request.Files["FileLocation"] != null
                   && !string.IsNullOrEmpty(Request.Files["FileLocation"].FileName))
                    {
                        _helperFunction.DeleteFile(invoiceToUpdate.Location);
                        string invoicePath = MoveInvoiceFile(invoice.InvoiceNumber);

                        invoice.Location = invoicePath;
                    }
                    dataClass.UpdateInvoice(invoiceToUpdate);
                }

                return RedirectToAction("Index", "Employee", new
                {
                    page = "Detail"
                });
            }
        }

        [HttpPost]
        public ActionResult CreateInvoice(Invoice invoice, FormCollection collection, string email)
        {
            using (DataClass dataClass = new DataClass())
            {

                if (ModelState.IsValid)
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

                    invoice.Items = items;
                    invoice.DateSent = DateTime.Now;
                    invoice.InvoiceNumber = string.Format("{0}", 1000 + dataClass.GetInvoiceCount());
                    invoice.Status = Status.Unpaid.ToString();

                    invoice.Customer = dataClass.GetCustomer(DataClass.CustomerId);

                    //Create PDF Documentcresta
                    string attachment = _generator.CreateInvoice(invoice);

                    invoice.Location = attachment;
                    //Send Email With Invoice.
                    _helperFunction.SendInvoiceEmail(email, invoice.Description, attachment);

                    invoice.Customer = null;
                    //Save Invoice In the Database
                    dataClass.CreateInvoice(DataClass.CustomerId, invoice);
                }
                return RedirectToAction("Index", "Employee", new
                {
                    page = "Detail"
                });
            }
        }

        [HttpPost]
        public ActionResult CreateOnceOffInvoice(OnceOffInvoice invoice, FormCollection collection, string email)
        {
            using (DataClass dataClass = new DataClass())
            {

                if (ModelState.IsValid)
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

                    invoice.Items = items;
                    invoice.DateSent = DateTime.Now;
                    invoice.InvoiceNumber = string.Format("{0}", 1000 + dataClass.GetInvoiceCount());
                    invoice.Status = Status.Unpaid.ToString();

                    //Create PDF Documentcresta
                    string attachment = _generator.CreateOnceOffInvoice(invoice);

                    invoice.Location = attachment;
                    //Send Email With Invoice.
                    _helperFunction.SendInvoiceEmail(email, invoice.Description, attachment);

                    //Save Invoice In the Database
                    dataClass.CreateOnceOffInvoice(invoice);
                }
                return RedirectToAction("Index", "Employee", new
                {
                    page = "MoreFuncation"
                });
            }
        }

        public string MoveInvoiceFile(string invoiceNumber)
        {
            string resourcePath = "Location";
            if (!Directory.Exists(Server.MapPath("~/Invoice/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Invoice/"));
            }

            if (Request.Files.Count > 0 && Request.Files["FileLocation"] != null
                && !string.IsNullOrEmpty(Request.Files["FileLocation"].FileName))
            {
                string fileName = Request.Files["FileLocation"].FileName;
                string extension = Path.GetExtension(fileName);

                resourcePath = Path.Combine(Server.MapPath("~/Invoice/"),
                    string.Format("{0}{1}", invoiceNumber, extension));

                if (!System.IO.File.Exists(resourcePath))
                {
                    Request.Files["FileLocation"].SaveAs(resourcePath);
                }
                else
                {
                    resourcePath = "Location";
                }
            }
            return resourcePath;
        }
    }
}