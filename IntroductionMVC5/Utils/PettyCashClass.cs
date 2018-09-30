using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.Mvc;
using IntroductionMVC5.Data;
using IntroductionMVC5.Models.PettyCash;
using IntroductionMVC5.Web.Utils.PagedList;
using IntroductionMVC5.Web.ViewModel;

namespace IntroductionMVC5.Web.Utils
{
    public class PettyCashClass
    {

        private const int CurrentPageIndex = 0;

        private readonly int _defaultPageSize =
            Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPaginationSize"]);

        private readonly ApplicationUnit _unit = new ApplicationUnit();
        public string Bank { get; set; }
        public string ChequeOrCard { get; set; }

        /// <summary>
        ///     Add New Payment to the account
        /// </summary>
        /// <param name="payment">New payment to add</param>
        /// <param name="account">Petty Cash Account</param>
        public void AddPayment(Payment payment, Account account)
        {
            List<PaymentType> paymentTypes = _unit.PaymentTypes.GetAll()
                .OrderBy(p => p.Type).ToList();
            payment.PaymentType = paymentTypes.FirstOrDefault(s => s.Id == payment.PaymentType.Id);

            if (payment.PaymentType != null &&
                (payment.Account != null &&
                 String.Compare(payment.PaymentType.Type, "Loan", StringComparison.OrdinalIgnoreCase) == 0))
            {
                Account accountToCredit = GetAllAccounts().FirstOrDefault(d => d.Id == payment.Account.Id);
                accountToCredit.Balance = accountToCredit.Balance + payment.Amount;
                payment.Account = accountToCredit;
                if (account != null)
                {
                    account.Balance = account.Balance - payment.Amount;
                    payment.AutoGenNumber = GetReferenceNumber();
                    payment.PettyAccount = account.Balance;
                    payment.IsCleared = !string.IsNullOrEmpty(payment.PastelNo);
                    _unit.Payments.Add(payment);
                    _unit.Accounts.Update(account);
                }
                _unit.Accounts.Update(accountToCredit);
                _unit.SaveChanges();
            }
            else
            {
                if (account != null)
                {
                    account.Balance = account.Balance - payment.Amount;
                    payment.AutoGenNumber = GetReferenceNumber();
                    payment.PettyAccount = account.Balance;
                    payment.IsCleared = !string.IsNullOrEmpty(payment.PastelNo);
                    payment.Account = null;
                    _unit.Payments.Add(payment);
                    _unit.Accounts.Update(account);
                }
                _unit.SaveChanges();
            }
        }

        /// <summary>
        ///     Add New Receipt to the account
        /// </summary>
        /// <param name="receipt">New payment to add</param>
        /// <param name="pettyCashAccount">Petty Cash Account</param>
        public void AddReceipt(Receipt receipt, Account pettyCashAccount)
        {
            receipt.AutoGenNumber = GetReferenceNumber();
            if (receipt.Type == "Sale")
            {
                pettyCashAccount.Balance = pettyCashAccount.Balance + receipt.Amount;
                receipt.PettyAccount = pettyCashAccount.Balance;
                _unit.Receipts.Add(receipt);
                _unit.Accounts.Update(pettyCashAccount);
                _unit.SaveChanges();
            }

            if (receipt.Type == "Loan")
            {
                Account account = _unit.Accounts.GetAll().FirstOrDefault(d => d.Id == receipt.Account.Id);
                account.Balance = account.Balance - receipt.Amount;

                pettyCashAccount.Balance = pettyCashAccount.Balance + receipt.Amount;
                receipt.PettyAccount = pettyCashAccount.Balance;
                receipt.Account = account;
                receipt.Reference = account.Name;
                _unit.Receipts.Add(receipt);
                _unit.Accounts.Update(pettyCashAccount);
                _unit.Accounts.Update(account);
                _unit.SaveChanges();
            }

            if (receipt.Type == "Bank")
            {
                pettyCashAccount.Balance = pettyCashAccount.Balance + receipt.Amount;
                receipt.ExtraInfo = receipt.ExtraInfo;
                receipt.Reference = string.Format("{0}({1})", Bank, ChequeOrCard);
                //receipt.ReceiptDate = DateTime.Now;
                receipt.PettyAccount = pettyCashAccount.Balance;
                _unit.Receipts.Add(receipt);
                _unit.Accounts.Update(pettyCashAccount);
                _unit.SaveChanges();
            }
        }

        /// <summary>
        ///     Edit Payment
        /// </summary>
        /// <param name="pymnt">Payment to edit</param>
        /// <param name="account">Petty Cash Account</param>
        public void EditPayment(Payment pymnt, Account account)
        {
            Payment payment = _unit.Payments.GetAll().FirstOrDefault(p => p.Id == pymnt.Id);

            List<PaymentType> paymentTypes = _unit.PaymentTypes.GetAll()
                .OrderBy(p => p.Type).ToList();

            payment.PaymentType = paymentTypes.FirstOrDefault(s => s.Id == pymnt.PaymentType.Id);
            payment.Description = pymnt.Description;
            if (payment.PaymentType != null &&
                (payment.Account != null &&
                 String.Compare(payment.PaymentType.Type, "Loan", StringComparison.OrdinalIgnoreCase) == 0))
            {
                Account accountToCredit = GetAllAccounts().FirstOrDefault(d => d.Id == payment.Account.Id);
                if (account != null && payment.Amount != pymnt.Amount)
                {
                    accountToCredit.Balance = accountToCredit.Balance - payment.Amount + pymnt.Amount;
                    payment.Account = accountToCredit;
                    account.Balance = account.Balance + payment.Amount - pymnt.Amount;
                    payment.PettyAccount = payment.PettyAccount + payment.Amount - pymnt.Amount;
                    payment.Amount = pymnt.Amount;
                }
                if (!string.IsNullOrEmpty(pymnt.PastelNo))
                {
                    payment.PastelNo = pymnt.PastelNo;
                    payment.IsCleared = true;
                }
                _unit.Payments.Update(payment);
                _unit.Accounts.Update(account);
                _unit.Accounts.Update(accountToCredit);
                _unit.SaveChanges();
            }
            else
            {
                if (account != null && payment.Amount != pymnt.Amount)
                {
                    account.Balance = account.Balance + payment.Amount - pymnt.Amount;
                    payment.PettyAccount = payment.PettyAccount + payment.Amount - pymnt.Amount;
                    payment.Amount = pymnt.Amount;
                }
                if (!string.IsNullOrEmpty(pymnt.PastelNo))
                {
                    payment.PastelNo = pymnt.PastelNo;
                    payment.IsCleared = true;
                }
                payment.Account = null;
                _unit.Payments.Update(payment);
                _unit.Accounts.Update(account);
                _unit.SaveChanges();
            }
        }

        /// <summary>
        ///     Edit Reciept
        /// </summary>
        /// <param name="rcpt">Reciept to edit</param>
        /// <param name="pettyCashAccount">Petty Cash Account</param>
        public void EditReciept(Receipt rcpt, Account pettyCashAccount)
        {
            Receipt receipt = _unit.Receipts.GetAll().Include(acc => acc.Account).FirstOrDefault(p => p.Id == rcpt.Id);

            if (rcpt.Type == "Sale" && receipt != null)
            {
                if (receipt.Amount != rcpt.Amount)
                {
                    pettyCashAccount.Balance = pettyCashAccount.Balance - receipt.Amount + rcpt.Amount;
                    receipt.PettyAccount = receipt.PettyAccount - receipt.Amount + rcpt.Amount;
                    receipt.Amount = rcpt.Amount;
                    _unit.Accounts.Update(pettyCashAccount);
                }
                receipt.Reference = rcpt.Reference;
                _unit.Receipts.Update(receipt);
                _unit.SaveChanges();
            }
            if (rcpt.Type == "Loan" && receipt != null)
            {
                Account account = _unit.Accounts.GetAll().FirstOrDefault(d => d.Id == receipt.Account.Id);
                account.Balance = account.Balance + receipt.Amount - rcpt.Amount;

                pettyCashAccount.Balance = pettyCashAccount.Balance - receipt.Amount + rcpt.Amount;
                receipt.PettyAccount = receipt.PettyAccount - receipt.Amount + rcpt.Amount;
                //receipt.ReceiptDate = DateTime.Now;
                receipt.Account = account;
                receipt.Amount = rcpt.Amount;
                receipt.Reference = account.Name;
                receipt.PastelNo = rcpt.PastelNo;
                receipt.IsCleared = !string.IsNullOrEmpty(rcpt.PastelNo);
                _unit.Receipts.Update(receipt);
                _unit.Accounts.Update(pettyCashAccount);
                _unit.Accounts.Update(account);
                _unit.SaveChanges();
            }
            if (rcpt.Type == "Bank" && receipt != null)
            {
                pettyCashAccount.Balance = pettyCashAccount.Balance - receipt.Amount + rcpt.Amount;
                receipt.PettyAccount = receipt.PettyAccount - receipt.Amount + rcpt.Amount;
                receipt.ExtraInfo = rcpt.ExtraInfo;
                receipt.Reference = string.Format("{0}", rcpt.Reference);
                //receipt.ReceiptDate = DateTime.Now;
                _unit.Receipts.Update(receipt);
                _unit.Accounts.Update(pettyCashAccount);
                _unit.SaveChanges();
            }
        }

        public PettyCashViewModel Search(string date)
        {
            DateTime searchDate = Convert.ToDateTime(date);

            //DateTime previousDayDate = searchDate.AddDays(-1);

            EndDayBalance previousDayBalance =
                _unit.EndDayBalances.GetAll()
                    .Where(d => EntityFunctions.TruncateTime(d.Date) <= EntityFunctions.TruncateTime(searchDate))
                    .OrderByDescending(b => b.Id)
                    .FirstOrDefault();

            bool lastDayBalanced = LastDayBalanced(previousDayBalance);
            EndDayBalance balanceInfo = _unit.EndDayBalances.GetAll()
                .FirstOrDefault(p => EntityFunctions.TruncateTime(p.Date)
                                     == EntityFunctions.TruncateTime(searchDate));


            List<Payment> payments = _unit.Payments
                .GetAll().Include(a => a.PaymentType)
                .Where(p => EntityFunctions.TruncateTime(p.Date) == EntityFunctions.TruncateTime(searchDate)
                && p.PaymentType != null).ToList();


            List<Receipt> receipts = _unit.Receipts
                .GetAll().Where(p => EntityFunctions.TruncateTime(p.Date) == EntityFunctions.TruncateTime(searchDate)
                && p.Type != null).ToList();

            List<IPettyCashEntry> dailyActivityList = payments.Cast<IPettyCashEntry>().ToList();
            dailyActivityList.AddRange(receipts);

            var pettyCashViewModel = new PettyCashViewModel
            {
                DailyActiviList = dailyActivityList.OrderBy(d => d.Date).ToPagedList(CurrentPageIndex,
                _defaultPageSize),
                PettyCashAccount = GetAllAccounts().FirstOrDefault(d => d.Name == "PETTY CASH"),
                EndDayBalance = previousDayBalance ?? new EndDayBalance(),
                BalanceInfomation = balanceInfo,
                PettyCashDate = searchDate,
                Accounts = GetAllAccounts(),
                IsLastDayBalanced = lastDayBalanced
            };

            return pettyCashViewModel;
        }

        private string GetReferenceNumber()
        {
            Payment lastPayment = _unit.Payments.GetAll().OrderByDescending(d => d.Id).FirstOrDefault();
            Receipt lastReceipt = _unit.Receipts.GetAll().OrderByDescending(d => d.Id).FirstOrDefault();
            string referenceNumber;
            if (lastPayment != null && lastReceipt != null)
            {
                int pymnt = Convert.ToInt32(lastPayment.AutoGenNumber.Split('-')[1]);
                int rcpt = Convert.ToInt32(lastReceipt.AutoGenNumber.Split('-')[1]);

                int refNo = pymnt > rcpt ? pymnt : rcpt;

                referenceNumber = string.Format("REF-{0}", refNo + 1);
            }
            else
            {
                lastPayment = _unit.Payments.GetAll().OrderByDescending(d => d.Id).FirstOrDefault();
                referenceNumber = string.Format("REF-{0}", lastPayment != null ? lastPayment.Id + 1 : 1);
            }
            return referenceNumber;
        }

        public List<Account> GetAllAccounts()
        {
            List<Account> accounts = _unit.Accounts.GetAll()
                .OrderBy(s => s.Name)
                .ToList();
            return accounts;
        }

        public bool LastDayBalanced(EndDayBalance yesterDayBalance)
        {
           
            Payment lastPayment = _unit.Payments.GetAll().OrderByDescending(b => b.Id).FirstOrDefault();
            Receipt lastReceipt = _unit.Receipts.GetAll().OrderByDescending(b => b.Id).FirstOrDefault();

            if (lastPayment != null && lastPayment.Date.Date != DateTime.Now.Date)
            {
                return true; // (lastPayment.PaymentDate.Date == yesterDayBalance.Date.Date);
                //|| (lastReceipt != null && lastReceipt.ReceiptDate.Date >= yesterDayBalance.Date.Date);
            }
            return true;
        }
    }
}