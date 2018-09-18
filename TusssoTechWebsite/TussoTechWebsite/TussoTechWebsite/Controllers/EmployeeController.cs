using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using TussoTechWebsite.Model;
using TussoTechWebsite.Models;
using TussoTechWebsite.PDFGenerator;

namespace TussoTechWebsite.Controllers
{
    public class EmployeeController : Controller
    {
        private static string _message = "";
        private readonly InvoiceGenerator _generator = new InvoiceGenerator();
        private HelperFunction _fileProcessing = new HelperFunction();

        // GET: Employee
        public ActionResult Index(string message = "", string page = "Index")
        {
            ViewBag.View = page;
            _message = message;

            return View();
        }

        [HttpGet]
        public ActionResult Details(int id = 0)
        {
            if (id != 0)
            {
                DataClass.CustomerId = id;
            }
            using (DataClass dataClass = new DataClass())
            {
                List<Qoutation> qoutations = dataClass.GetQoutations(DataClass.CustomerId);
                ViewBag.Qoutations = qoutations;
                return PartialView("_Details", dataClass.GetCustomer(DataClass.CustomerId));
            }
        }

        [HttpGet]
        public ActionResult CompanyDocuments()
        {
            using (DataClass dataClass = new DataClass())
            {
                ViewBag.DocumentTypes = new SelectList(dataClass.DocumentTypes);

                return PartialView("_CDocuments", dataClass.GetCompany());
            }
        }

        [HttpGet]
        public ActionResult BankStatements()
        {
            ViewBag.Message = _message;
            using (DataClass dataClass = new DataClass())
            {
                return PartialView("_BankStatements", dataClass.GetBankStatement());
            }
        }

        [HttpGet]
        public ActionResult Company()
        {
            return PartialView("_Company");
        }

        [HttpGet]
        public ActionResult Customers()
        {
            using (DataClass dataClass = new DataClass())
            {
                return PartialView("_Customers", dataClass.GetCustomers());
            }
        }

        [HttpGet]
        public ActionResult ResourceMaterial()
        {
            using (DataClass dataClass = new DataClass())
            {
                return PartialView("_ResourceMaterial", dataClass.GetCompany());
            }
        }

        [HttpPost]
        public ActionResult CreateCustomer(Customer customer)
        {
            if (ModelState.IsValid)
            {
                using (DataClass dataClass = new DataClass())
                {
                    dataClass.AddCustomer(customer);
                }
                return RedirectToAction("Index", new
                {
                    message = "New Customer Added Successfully"
                });
            }
            return RedirectToAction("Index", new
            {
                message = "Failed to Add New customer"
            });
        }

        [HttpGet]
        public ActionResult EditCustomer(int id)
        {
            using (DataClass dataClass = new DataClass())
            {
                var selectedCustomer = dataClass.GetCustomer(id);

                return PartialView("_EditCustomer", selectedCustomer);
            }
        }

        [HttpGet]
        public ActionResult EditDocument(int id)
        {
            using (DataClass dataClass = new DataClass())
            {
                var selectedDocument = dataClass.GetCompanyDocument(id);
                ViewBag.DocumentTypes = new SelectList(dataClass.DocumentTypes);

                return PartialView("_EditCDocument", selectedDocument);
            }
        }

        [HttpPost]
        public ActionResult EditCustomer(Customer customer)
        {
            if (ModelState.IsValid)
            {
                using (DataClass dataClass = new DataClass())
                {
                    Customer customerToUpdate = dataClass.GetCustomer(customer.Id);
                    customerToUpdate.Name = customer.Name;
                    customerToUpdate.EmailAddress = customer.EmailAddress;
                    customerToUpdate.VatNumber = customer.VatNumber;
                    customerToUpdate.Contact = customer.Contact;
                    customerToUpdate.Address = customer.Address;

                    dataClass.UpdateCustomer(customerToUpdate);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddResource(Resource resource)
        {
            if (ModelState.IsValid)
            {
                using (DataClass dataClass = new DataClass())
                {
                    resource.Customer = dataClass.GetCustomer(DataClass.CustomerId);
                    resource.Location = MoveFile(resource.Customer.Name, resource.Description);

                    dataClass.AddResource(resource);
                }
                return RedirectToAction("Index", new
                {
                    page = "Detail"
                });
            }
            return RedirectToAction("Index", new
            {
                page = "Detail"
            });
        }

        [HttpPost]
        public ActionResult AddTenderResource(Resource resource)
        {
            if (ModelState.IsValid)
            {
                using (DataClass dataClass = new DataClass())
                {
                    List<Resource> resources = dataClass.GetResourcesByType(DataClass.ResourceTypes.Tender.ToString());

                    Company company = dataClass.GetCompany();
                    resource.Location = MoveFile(company.Name, string.Format("Tender-{0}", resources.Count + 1));
                    resource.Type = DataClass.ResourceTypes.Tender.ToString();
                    company.Resources.Add(resource);

                    dataClass.UpdateCompany(company);
                }
                return RedirectToAction("Index", new
                {
                    page = "ResourceMaterial"
                });
            }
            return RedirectToAction("Index", new
            {
                page = "ResourceMaterial"
            });
        }

        [HttpPost]
        public ActionResult AddProposalResource(Resource resource)
        {
            if (ModelState.IsValid)
            {
                using (DataClass dataClass = new DataClass())
                {
                    List<Resource> resources = dataClass.GetResourcesByType(DataClass.ResourceTypes.Proposal.ToString());

                    Company company = dataClass.GetCompany();
                    resource.Location = MoveFile(company.Name, string.Format("Proposal-{0}", resources.Count + 1));
                    resource.Type = DataClass.ResourceTypes.Proposal.ToString();
                    company.Resources.Add(resource);

                    dataClass.UpdateCompany(company);
                }
                return RedirectToAction("Index", new
                {
                    page = "ResourceMaterial"
                });
            }
            return RedirectToAction("Index", new
            {
                page = "ResourceMaterial"
            });
        }

        [HttpPost]
        public ActionResult AddCompanyDoc(CompanyDocument document)
        {
            if (ModelState.IsValid)
            {
                using (DataClass dataClass = new DataClass())
                {
                    Company company = dataClass.GetCompany();
                    document.Location = MoveFile(company.Name, document.Name);
                    company.Documents.Add(document);

                    dataClass.UpdateCompany(company);
                }
                return RedirectToAction("Index", new
                {
                    page = "Company"
                });
            }
            return RedirectToAction("Index", new
            {
                page = "Company"
            });
        }

        [HttpPost]
        public ActionResult EditDocument(CompanyDocument document)
        {
            if (ModelState.IsValid)
            {
                using (DataClass dataClass = new DataClass())
                {
                    Company company = dataClass.GetCompany();
                    var selectedDocument = dataClass.GetCompanyDocument(document.Id);

                    if (Request.Files.Count > 0 && Request.Files["FileLocation"] != null
                        && !string.IsNullOrEmpty(Request.Files["FileLocation"].FileName))
                    {
                        _fileProcessing.DeleteFile(selectedDocument.Location);
                        selectedDocument.Location = MoveFile(company.Name, document.Name);
                    }
                    selectedDocument.Name = document.Name;

                    selectedDocument.Type = document.Type;

                    dataClass.UpdateCompanyDocument(selectedDocument);
                }
                return RedirectToAction("Index", new
                {
                    page = "Company"
                });
            }
            return RedirectToAction("Index", new
            {
                page = "Company"
            });
        }

        [HttpPost]
        public ActionResult AddBankStatement(BankStatement statement)
        {
            if (ModelState.IsValid)
            {
                using (DataClass dataClass = new DataClass())
                {
                    Company company = dataClass.GetCompany();
                    statement.Location = MoveFile(company.Name,
                        string.Format("{0}_{1}", "Statement", statement.DateSent.ToString("Y")));

                    if (statement.Location != "Location")
                    {
                        dataClass.AddBankStatment(statement);
                        return RedirectToAction("Index", new
                        {
                            page = "Statement",
                            message = "Successfully added bank statement"
                        });
                    }
                }
                return RedirectToAction("Index", new
                {
                    page = "Statement",
                    message = "Statement for the month already added"
                });
            }
            return RedirectToAction("Index", new
            {
                page = "Statement"
            });
        }

        [HttpGet]
        public ActionResult EditBankStatement(int id)
        {
            using (DataClass dataClass = new DataClass())
            {
                var selectedStatement = dataClass.GetBankStatement(id);

                return PartialView("_EditBankStatement", selectedStatement);
            }
        }

        [HttpPost]
        public ActionResult EditBankStatement(BankStatement statement)
        {
            if (ModelState.IsValid)
            {
                using (DataClass dataClass = new DataClass())
                {
                    Company company = dataClass.GetCompany();
                    BankStatement statementToUpdate = dataClass.GetBankStatement(statement.Id);

                    if (Request.Files.Count > 0 && Request.Files["FileLocation"] != null
                   && !string.IsNullOrEmpty(Request.Files["FileLocation"].FileName))
                    {
                        _fileProcessing.DeleteFile(statementToUpdate.Location);
                        statementToUpdate.Location = MoveFile(company.Name,
                        string.Format("{0}_{1}", "Statement", statement.DateSent.ToString("Y")));
                    }
                    statementToUpdate.AccountAmount = statement.AccountAmount;

                    dataClass.UpdateBankStatement(statementToUpdate);
                }
            }
            return RedirectToAction("Index", new
            {
                page = "Statement"
            });
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

        private string MoveFile(string customerName, string description)
        {
            string resourcePath = "Location";
            if (!Directory.Exists(Server.MapPath("~/Resource/" + customerName)))
            {
                Directory.CreateDirectory(Server.MapPath("~/Resource/" + customerName));
            }

            if (Request.Files.Count > 0 && Request.Files["FileLocation"] != null
                && !string.IsNullOrEmpty(Request.Files["FileLocation"].FileName))
            {
                string fileName = Request.Files["FileLocation"].FileName;
                string extension = Path.GetExtension(fileName);

                resourcePath = Path.Combine(Server.MapPath("~/Resource/" + customerName),
                    string.Format("{0}_{1}{2}", customerName, description, extension));

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