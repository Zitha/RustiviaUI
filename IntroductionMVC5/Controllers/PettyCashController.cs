using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web.Mvc;
using IntroductionMVC5.Data;
using IntroductionMVC5.Models.PettyCash;
using IntroductionMVC5.Web.Utils;
using IntroductionMVC5.Web.Utils.PagedList;
using IntroductionMVC5.Web.ViewModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using RustiviaSolutions.PDFGenerator;

namespace IntroductionMVC5.Web.Controllers
{
    [Authorize]
    public class PettyCashController : Controller
    {
        private const int CurrentPageIndex = 0;
        private static string _name;

        private readonly int _defaultPageSize =
            Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPaginationSize"]);

        private readonly PettyCashClass _pettyClass = new PettyCashClass();

        private readonly ApplicationUnit _unit = new ApplicationUnit();
        private List<Payment> _payments = new List<Payment>();

        public PettyCashController()
        {
            _payments = GetAllPayments();
        }

        private List<Payment> GetAllPayments()
        {
            List<Payment> payments = _unit.Payments.GetAll()
                .Where(p => EntityFunctions.TruncateTime(p.Date) == EntityFunctions.TruncateTime(DateTime.Now))
                .Include(p => p.PaymentType)
                .Include(a => a.Account)
                .OrderBy(s => s.Date)
                .ToList();
            return payments;
        }

        public ActionResult Index(bool? message)
        {
            List<PaymentType> paymentTypes = _unit.PaymentTypes.GetAll()
                .OrderBy(p => p.Type).ToList();

            ViewBag.paymentTypes = new SelectList(paymentTypes, "Id", "Type");
            ViewBag.Accounts = new SelectList(_pettyClass.GetAllAccounts().Where(d => d.Name != "PETTY CASH").ToList(),
                "Id", "Name");
            ViewBag.Banks = new SelectList(new List<string> { "Standard bank", "Pay card" });

            PettyCashViewModel pettyCashViewModel = _pettyClass.Search(DateTime.Now.ToString("d"));
            ViewBag.ClearAllMessage = message ?? false;

            return View(pettyCashViewModel);
        }

        public ActionResult EditPayment(int id)
        {
            List<PaymentType> paymentTypes = _unit.PaymentTypes.GetAll()
                .OrderBy(p => p.Type).ToList();

            Payment payment = _unit.Payments.GetAll().FirstOrDefault(p => p.Id == id);

            ViewBag.paymentTypes = new SelectList(paymentTypes, "Id", "Type");
            ViewBag.Accounts = new SelectList(_pettyClass.GetAllAccounts().Where(d => d.Name != "PETTY CASH").ToList(),
                "Id", "Name");
            ViewBag.Banks = new SelectList(new List<string> { "Standard bank", "Pay card" });
            return View(payment);
        }

        [HttpPost]
        public ActionResult EditPayment(Payment pymnt)
        {
            Payment payment = _unit.Payments.GetAll().FirstOrDefault(p => p.Id == pymnt.Id);

            List<PaymentType> paymentTypes = _unit.PaymentTypes.GetAll()
                .OrderBy(p => p.Type).ToList();
            Account account = _pettyClass.GetAllAccounts().FirstOrDefault(d => d.Name == "PETTY CASH");

            ICollection<string> keys = ModelState.Keys;
            foreach (string key in keys)
            {
                if (key.Contains("PaymentType"))
                {
                    ModelState[key].Errors.Clear();
                }
                if (key.Contains("Account"))
                {
                    ModelState[key].Errors.Clear();
                }
            }

            if (ModelState.IsValid)
            {
                _pettyClass.EditPayment(pymnt, account);

                _payments = GetAllPayments();
                return RedirectToAction("Search", payment.Date);
            }
            ViewBag.paymentTypes = new SelectList(paymentTypes, "Id", "Type");
            ViewBag.Accounts = new SelectList(_pettyClass.GetAllAccounts().Where(d => d.Name != "PETTY CASH").ToList(),
                "Id", "Name");
            ViewBag.Banks = new SelectList(new List<string> { "Standard bank", "Pay card" });
            return View(pymnt);
        }

        [HttpGet]
        public ActionResult CreatePayment()
        {
            List<PaymentType> paymentTypes = _unit.PaymentTypes.GetAll()
                .OrderBy(p => p.Type).ToList();

            ViewBag.paymentTypes = new SelectList(paymentTypes, "Id", "Type");
            ViewBag.Accounts = new SelectList(_pettyClass.GetAllAccounts(), "Id", "Name");
            var payment = new Payment
            {
                Date = DateTime.Now
            };

            return View(payment);
        }

        [HttpPost]
        public ActionResult CreateReceipt(Receipt receipt)
        {
            receipt.User = User.Identity.Name;
            receipt.Date = DateTime.Now;
            Account pettyCashAccount = _unit.Accounts.GetAll().FirstOrDefault(d => d.Name == "PETTY CASH");
            if (receipt.Type == "Sale")
            {
                ICollection<string> keys = ModelState.Keys;
                foreach (string key in keys)
                {
                    if (key.Contains("Account"))
                    {
                        ModelState[key].Errors.Clear();
                    }
                }
                receipt.Account = null;
            }
            if (receipt.Type == "Bank")
            {
                ICollection<string> keys = ModelState.Keys;
                foreach (string key in keys)
                {
                    if (key.Contains("Account"))
                    {
                        ModelState[key].Errors.Clear();
                    }
                    if (key.Contains("Reference"))
                    {
                        ModelState[key].Errors.Clear();
                    }
                }
                receipt.Account = null;
            }
            if (receipt.Type == "Loan")
            {
                ICollection<string> keys = ModelState.Keys;
                foreach (string key in keys)
                {
                    if (key.Contains("Account.Name"))
                    {
                        ModelState[key].Errors.Clear();
                    }
                    if (key.Contains("Reference"))
                    {
                        ModelState[key].Errors.Clear();
                    }
                }
            }

            if (ModelState.IsValid && pettyCashAccount != null)
            {
                _pettyClass.Bank = Request["Bank"];
                _pettyClass.ChequeOrCard = Request["ChequeOrCard"];
                _pettyClass.AddReceipt(receipt, pettyCashAccount);

                return RedirectToAction("Index");
            }

            return View("Index");
        }

        [HttpPost]
        public ActionResult CreatePayment(Payment payment)
        {
            List<PaymentType> paymentTypes = _unit.PaymentTypes.GetAll()
                .OrderBy(p => p.Type).ToList();
            Account account = _pettyClass.GetAllAccounts().FirstOrDefault(d => d.Name == "PETTY CASH");

            payment.User = User.Identity.Name;
            payment.Date = DateTime.Now;

            ICollection<string> keys = ModelState.Keys;
            foreach (string key in keys)
            {
                if (key.Contains("PaymentType"))
                {
                    ModelState[key].Errors.Clear();
                }
                if (key.Contains("Account"))
                {
                    ModelState[key].Errors.Clear();
                }
            }

            if (ModelState.IsValid)
            {
                _pettyClass.AddPayment(payment, account);

                _payments = GetAllPayments();
                return RedirectToAction("Index");
            }
            ViewBag.paymentTypes = new SelectList(paymentTypes, "Id", "Type");
            return View(payment);
        }

        public ActionResult AccountHistory(string name, int? page)
        {
            _name = name ?? _name;
            Account account = _pettyClass.GetAllAccounts().FirstOrDefault(d => d.Name == _name);
            List<Payment> payments = _name == "PETTY CASH"
                ? _unit.Payments.GetAll().Include(d => d.PaymentType).ToList()
                : _unit.Payments.GetAll()
                    .Include(pt => pt.PaymentType)
                    .Where(p => p.Account != null && p.Account.Id == account.Id)
                    .ToList();

            List<Receipt> receipts = _name == "PETTY CASH"
                ? _unit.Receipts.GetAll().ToList()
                : _unit.Receipts.GetAll().Where(p => p.Account != null && p.Account.Id == account.Id).ToList();

            List<IPettyCashEntry> accountActivity = payments.Cast<IPettyCashEntry>().ToList();
            accountActivity.AddRange(receipts);


            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            var pettyCashViewModel = new PettyCashViewModel
            {
                DailyActiviList = accountActivity.OrderByDescending(d => d.Date).ToPagedList(currentPageIndex,
                    _defaultPageSize),
                PettyCashAccount = account
            };

            return View(pettyCashViewModel);
        }

        [HttpPost]
        public ActionResult CreateAccount(Account account)
        {
            if (ModelState.IsValid)
            {
                _unit.Accounts.Add(account);
                _unit.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        [HttpPost]
        public ActionResult CreatePaymentType(PaymentType paymentType)
        {
            if (ModelState.IsValid)
            {
                _unit.PaymentTypes.Add(paymentType);
                _unit.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Index");
        }

        public ActionResult BalancePettyCash(string pettyCashDate)
        {
            if (!string.IsNullOrEmpty(pettyCashDate))
            {
                DateTime balanceDate = Convert.ToDateTime(pettyCashDate);
                Account pettyCashAccount = _pettyClass.GetAllAccounts().FirstOrDefault(d => d.Name == "PETTY CASH");
                EndDayBalance yesterDayBalance = _unit.EndDayBalances.GetAll().ToList().LastOrDefault();

                var dayBalance = new EndDayBalance
                {
                    Date = balanceDate,
                    ClosingBalance = pettyCashAccount.Balance,
                    SystemUser = System.Web.HttpContext.Current.User.Identity.Name,
                    OpeningBalance = yesterDayBalance != null ? yesterDayBalance.ClosingBalance : 0
                };

                _unit.EndDayBalances.Add(dayBalance);
                _unit.SaveChanges();

                return RedirectToAction("Search", new { date = pettyCashDate });
            }
            return RedirectToAction("Index");
        }

        public ActionResult Search(string date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                List<PaymentType> paymentTypes = _unit.PaymentTypes.GetAll()
                    .OrderBy(p => p.Type).ToList();

                ViewBag.paymentTypes = new SelectList(paymentTypes, "Id", "Type");
                ViewBag.Accounts =
                    new SelectList(_pettyClass.GetAllAccounts().Where(d => d.Name != "PETTY CASH").ToList(), "Id",
                        "Name");
                ViewBag.Banks = new SelectList(new List<string> { "Standard bank", "Pay card" });
                ViewBag.ClearAllMessage = false;
                PettyCashViewModel pettyCashViewModel = _pettyClass.Search(date);
                if (Request.IsAjaxRequest())
                    return PartialView("Index", pettyCashViewModel);
                return View("Index", pettyCashViewModel);
            }
            return RedirectToAction("Index");
        }

        public ActionResult PrintPayementReciept(int id)
        {
            var print = new PettyCashPrint();
            Payment payment = _unit.Payments.GetAll()
                .Include(pymnt => pymnt.PaymentType)
                .FirstOrDefault(p => p.Id == id);
            print.PrintPaymentReceipt(payment);

            if (payment != null)
            {
                return RedirectToAction("Search", payment.Date);
            }
            return RedirectToAction("Index");
        }

        public ActionResult PrintReciept(int id)
        {
            var print = new PettyCashPrint();
            Receipt receipt = _unit.Receipts.GetAll().FirstOrDefault(p => p.Id == id);
            print.PrintReceipt(receipt);

            if (receipt != null)
            {
                return RedirectToAction("Search", receipt.Date);
            }
            return RedirectToAction("Index");
        }

        public ActionResult PrintDayEnd(DateTime pettycashdate)
        {
            List<PaymentType> paymentTypes = _unit.PaymentTypes.GetAll()
                .OrderBy(p => p.Type).ToList();

            ViewBag.paymentTypes = new SelectList(paymentTypes, "Id", "Type");
            ViewBag.Accounts = new SelectList(_pettyClass.GetAllAccounts().Where(d => d.Name != "PETTY CASH").ToList(),
                "Id", "Name");
            ViewBag.Banks = new SelectList(new List<string> { "Standard bank", "Pay card" });

            PettyCashViewModel pettyCashViewModel = _pettyClass.Search(pettycashdate.ToString("d"));

            string pdf = CreatePdf(pettyCashViewModel);

            using (var webClient = new WebClient())
            {
                if (!System.IO.File.Exists(pdf))
                {
                    return HttpNotFound();
                }
                byte[] file = webClient.DownloadData(pdf);
                return File(file, MediaTypeNames.Application.Pdf);
            }
        }

        private string CreatePdf(PettyCashViewModel pettyCashViewModel)
        {
            var doc = new Document(PageSize.A4);

            if (!Directory.Exists(Server.MapPath("~/PettyCash")))
            {
                Directory.CreateDirectory(Server.MapPath("~/PettyCash"));
            }
            string currpath = string.Format("{0}/{1}.pdf", Server.MapPath("~/PettyCash"),
                System.Web.HttpContext.Current.User.Identity.Name);
            var output = new FileStream(currpath, FileMode.Create);

            PdfWriter writer = PdfWriter.GetInstance(doc, output);

            doc.Open();

            var table = new PdfPTable(2) { HorizontalAlignment = Element.ALIGN_MIDDLE, WidthPercentage = 80 };

            var chunk = new Chunk("Petty Cash Report", FontFactory.GetFont("dax-black"));
            chunk.SetUnderline(0.5f, -1.5f);

            var p2 = new Phrase { chunk };
            var p = new Paragraph { p2 };

            doc.Add(p);

            var dateCaption =
                new PdfPCell(new Phrase("Date",
                    FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.NORMAL)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    PaddingBottom = 10f,
                    VerticalAlignment = Element.ALIGN_CENTER,
                    BorderColor = BaseColor.WHITE,
                };
            table.AddCell(dateCaption);
            var date = new PdfPCell(new Phrase(pettyCashViewModel.BalanceInfomation.Date.ToString("d"),
                FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL)))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_CENTER,
                BorderColor = BaseColor.WHITE,
                PaddingBottom = 1f
            };
            table.AddCell(date);

            //Header Table
            var table1 = new PdfPTable(5);
            var openingBalanceCaption =
                new PdfPCell(new Phrase("Opening balance",
                    FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD)))
                {
                    Colspan = 4,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    PaddingBottom = 5f
                };
            table1.AddCell(openingBalanceCaption);
            var openingBalance = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            openingBalance.AddElement(
                new Paragraph(
                    string.Format("R {0}",
                        pettyCashViewModel.BalanceInfomation.OpeningBalance.ToString(CultureInfo.InvariantCulture)),
                    FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL)));
            table1.AddCell(openingBalance);


            //Header Cells
            var hdrType =
                new PdfPCell(new Phrase("Type",
                    FontFactory.GetFont(FontFactory.HELVETICA, 14, Font.BOLD)))
                {
                    HorizontalAlignment = 1,
                    PaddingBottom = 5f
                };
            var hdrDescription =
                new PdfPCell(new Phrase("Description",
                    FontFactory.GetFont(FontFactory.HELVETICA, 14, Font.BOLD)))
                {
                    HorizontalAlignment = 1,
                    PaddingBottom = 5f
                };
            var hdrPastelNo =
                new PdfPCell(new Phrase("Pastel No",
                    FontFactory.GetFont(FontFactory.HELVETICA, 14, Font.BOLD)))
                {
                    HorizontalAlignment = 1,
                    PaddingBottom = 5f
                };
            var hdrRefNo =
                new PdfPCell(new Phrase("Ref No",
                    FontFactory.GetFont(FontFactory.HELVETICA, 14, Font.BOLD)))
                {
                    HorizontalAlignment = 1,
                    PaddingBottom = 5f
                };
            var hdrAmount =
                new PdfPCell(new Phrase("Amount",
                    FontFactory.GetFont(FontFactory.HELVETICA, 14, Font.BOLD)))
                {
                    HorizontalAlignment = 1,
                    PaddingBottom = 5f
                };
            table1.AddCell(hdrType);
            table1.AddCell(hdrDescription);
            table1.AddCell(hdrPastelNo);
            table1.AddCell(hdrRefNo);
            table1.AddCell(hdrAmount);
            foreach (IPettyCashEntry activity in pettyCashViewModel.DailyActiviList)
            {
                if (activity.GetType() == typeof(Payment))
                {
                    var payment = (Payment)activity;
                    var cell1 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
                    var cell2 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
                    var cell3 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
                    var cell4 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
                    var cell5 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
                    cell1.AddElement(new Paragraph(payment.PaymentType.Type,
                        FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL)));
                    cell2.AddElement(new Paragraph(payment.Description,
                        FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL)));
                    cell3.AddElement(new Paragraph(payment.PastelNo,
                        FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL)));
                    cell4.AddElement(new Paragraph(payment.AutoGenNumber,
                        FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL)));
                    cell5.AddElement(
                        new Paragraph(string.Format("R -{0}", payment.Amount.ToString(CultureInfo.InvariantCulture)),
                            FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL)));

                    table1.AddCell(cell1);
                    table1.AddCell(cell2);
                    table1.AddCell(cell3);
                    table1.AddCell(cell4);
                    table1.AddCell(cell5);
                }
                if (activity.GetType() == typeof(Receipt))
                {
                    var receipt = (Receipt)activity;
                    var cell1 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
                    var cell2 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
                    var cell3 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
                    cell1.AddElement(new Paragraph(receipt.Type,
                        FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL)));
                    cell2.AddElement(new Paragraph(receipt.Reference,
                        FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL)));
                    cell3.AddElement(
                        new Paragraph(string.Format("R {0}", receipt.Amount.ToString(CultureInfo.InvariantCulture)),
                            FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL)));

                    table1.AddCell(cell1);
                    table1.AddCell(cell2);
                    table1.AddCell(string.Empty);
                    table1.AddCell(string.Empty);
                    table1.AddCell(cell3);
                }
            }
            var closingBalance =
                new PdfPCell(new Phrase("Closing balance",
                    FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD)))
                {
                    Colspan = 4,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    PaddingBottom = 5f
                };
            table1.AddCell(closingBalance);
            var stuff = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            stuff.AddElement(
                new Paragraph(
                    string.Format("R {0}",
                        pettyCashViewModel.BalanceInfomation.ClosingBalance.ToString(CultureInfo.InvariantCulture)),
                    FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL)));
            table1.AddCell(stuff);
            doc.Add(table);
            doc.Add(table1);
            doc.Close();
            return currpath;
        }

        public ActionResult EditReceipt(int id)
        {
            Receipt receipt = _unit.Receipts.GetAll().FirstOrDefault(p => p.Id == id);

            ViewBag.Accounts = new SelectList(_pettyClass.GetAllAccounts().Where(d => d.Name != "PETTY CASH").ToList(),
                "Id", "Name");
            ViewBag.Banks = new SelectList(new List<string> { "Standard bank", "Pay card" });
            ViewBag.ReceiptType = new SelectList(new List<string> { "Loan", "Sale", "Bank" });
            return View(receipt);
        }

        [HttpPost]
        public ActionResult EditReceipt(Receipt rcpt)
        {
            Account pettyCashAccount = _unit.Accounts.GetAll().FirstOrDefault(d => d.Name == "PETTY CASH");
            if (rcpt.Type == "Sale")
            {
                ICollection<string> keys = ModelState.Keys;
                foreach (string key in keys)
                {
                    if (key.Contains("Account"))
                    {
                        ModelState[key].Errors.Clear();
                    }
                }
                rcpt.Account = null;
            }
            if (rcpt.Type == "Bank")
            {
                ICollection<string> keys = ModelState.Keys;
                foreach (string key in keys)
                {
                    if (key.Contains("Account"))
                    {
                        ModelState[key].Errors.Clear();
                    }
                    if (key.Contains("Reference"))
                    {
                        ModelState[key].Errors.Clear();
                    }
                }
                rcpt.Account = null;
            }
            if (rcpt.Type == "Loan")
            {
                ICollection<string> keys = ModelState.Keys;
                foreach (string key in keys)
                {
                    if (key.Contains("Account.Name"))
                    {
                        ModelState[key].Errors.Clear();
                    }
                    if (key.Contains("Reference"))
                    {
                        ModelState[key].Errors.Clear();
                    }
                }
            }

            if (ModelState.IsValid && pettyCashAccount != null)
            {
                _pettyClass.EditReciept(rcpt, pettyCashAccount);
                return RedirectToAction("Search", rcpt.Date);
            }

            ViewBag.Accounts = new SelectList(_pettyClass.GetAllAccounts().Where(d => d.Name != "PETTY CASH").ToList(),
                "Id", "Name");
            ViewBag.Banks = new SelectList(new List<string> { "Standard bank", "Pay card" });
            ViewBag.ReceiptType = new SelectList(new List<string> { "Loan", "Sale", "Bank" });
            return View(rcpt);
        }

        [HttpPost]
        public ActionResult ClearPayment(FormCollection collection)
        {
            var pastelNumber = collection["pastelNumber"];
            var id = Convert.ToInt32(collection["id"]);

            Payment unPayment = _unit.Payments.GetAll().FirstOrDefault(py => py.Id == id);
            if (unPayment != null)
            {
                unPayment.IsCleared = true;
                unPayment.PastelNo = pastelNumber;
                _unit.Payments.Update(unPayment);
            }
            _unit.SaveChanges();

            return RedirectToAction("AccountHistory", new { name = _name });
        }
    }
}