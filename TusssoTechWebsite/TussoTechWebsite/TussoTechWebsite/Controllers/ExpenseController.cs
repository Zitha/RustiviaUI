using System;
using System.Collections.Generic;
using System.Configuration;
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
    public class ExpenseController : Controller
    {
        private readonly InvoiceGenerator _generator = new InvoiceGenerator();
        private readonly DataClass _dataClass = new DataClass();
        private HelperFunction _helperFunction = new HelperFunction();

        [HttpGet]
        public ActionResult Expenditures()
        {
            ViewBag.Message = "";

            string[] exployees = ConfigurationManager.AppSettings["Employees"].Split(';');
            ViewBag.Employees = new SelectList(exployees);

            return PartialView("_Expenditures", _dataClass.GetExpenditures());
        }

        [HttpGet]
        public ActionResult EditExpense(int id)
        {
            string[] exployees = ConfigurationManager.AppSettings["Employees"].Split(';');
            ViewBag.Employees = new SelectList(exployees);

            var selectedInvocie = _dataClass.GetExpense(id);

            return PartialView("_EditExpenditure", selectedInvocie);
        }

        [HttpPost]
        public ActionResult EditExpense(Expense expense)
        {
            if (ModelState.IsValid)
            {
                Expense expenseToUpdate = _dataClass.GetExpense(expense.Id);

                if (Request.Files["FileLocation"] != null &&
                    !string.IsNullOrEmpty(Request.Files["FileLocation"].FileName))
                {
                    expense.Location = MoveFile(expenseToUpdate.Company.Name, expense.PurchaseNumber);
                }
                else
                {
                    _helperFunction.DeleteFile(expenseToUpdate.Location);
                    expenseToUpdate.Location = _generator.CreateExpense(expense);
                }
                expenseToUpdate.Total = expense.Total;
                expenseToUpdate.Type = expense.Type;
                expenseToUpdate.Employee = expense.Employee;
                expenseToUpdate.Description = expense.Description;

                _dataClass.UpdateExpense(expenseToUpdate);
                return RedirectToAction("Index", new
                {
                    page = "Expense"
                });
            }
            return RedirectToAction("Index", "Employee", new
            {
                page = "Expense"
            });
        }

        [HttpPost]
        public ActionResult AddExpense(Expense expense)
        {
            if (ModelState.IsValid)
            {
                Company company = _dataClass.GetCompany();
                expense.Company = company;
                expense.PurchaseNumber = string.Format("{0}_{1}", "EXP", 100 + _dataClass.GetExpenseCount());
                if (Request.Files["FileLocation"] != null &&
                    !string.IsNullOrEmpty(Request.Files["FileLocation"].FileName))
                {
                    expense.Location = MoveFile(company.Name, expense.PurchaseNumber);
                }
                else
                {
                    expense.Location = _generator.CreateExpense(expense);
                }

                _dataClass.AddExpense(expense);
            }
            return RedirectToAction("Index", "Employee", new
            {
                page = "Expense"
            });
        }

        [HttpPost]
        public ActionResult DeleteExpense(FormCollection collection)
        {
            var id = Convert.ToInt32(collection["expenseID"]);

            var expense = _dataClass.GetExpense(id);

            if (expense != null)
            {
                _dataClass.DeleteExpenditure(id);
            }

            return RedirectToAction("Index", "Employee", new
            {
                page = "Expense"
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
                    return File(file, filetype, Path.GetFileName(fileName));
                }
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