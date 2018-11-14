using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TussoTechWebsite.Model;
using TussoTechWebsite.Models;
using TussoTechWebsite.PDFGenerator;

namespace TussoTechWebsite.Controllers
{
    public class FinanceController : Controller
    {
        private readonly InvoiceGenerator _generator = new InvoiceGenerator();
        //private readonly DataClass dataClass = new DataClass();
        private HelperFunction _helperFunction = new HelperFunction();

        // GET: Finance
        public ActionResult Index()
        {
            using (DataClass dataClass = new DataClass())
            {
                Reporting report = new Reporting();
                var allInvoice = dataClass.GetAllInvoice();
                var allExpense = dataClass.GetAllExpense();

                report.Invoices = allInvoice;
                report.Expenses = allExpense;

                return PartialView("_Finance", report);
            }
        }

        public PartialViewResult OutstandingInvoice()
        {
            using (DataClass dataClass = new DataClass())
            {
                var unpaidInvoice = dataClass.GetAllInvoice(Status.Unpaid.ToString());
                return PartialView("_OutstandingInvoice", unpaidInvoice);
            }
        }
    }

    public class Reporting
    {
        public List<Invoice> Invoices
        {
            get;
            set;
        }

        public List<Expense> Expenses
        {
            get;
            set;
        }
    }
}