using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TussoTechWebsite.Data;
using TussoTechWebsite.Model;

namespace TussoTechWebsite.Models
{
    public class DataClass : IDisposable
    {
        ApplicationUnit _unit = new ApplicationUnit();
        public static int CustomerId
        {
            get;
            set;
        }
        public readonly List<string> DocumentTypes = new List<string>
        {
            "Bee",
            "Tax Clearence",
            "Company Registration",
            "Bank Statement",
            "Meeting Minute"
        };

        public enum ResourceTypes
        {
            Tender,
            Proposal
        };

        public List<Customer> GetCustomers()
        {
            return _unit.Customers.GetAll().ToList();
        }

        public void AddCustomer(Customer customer)
        {
            _unit.Customers.Add(customer);
            _unit.SaveChanges();
        }

        public Customer GetCustomer(int id)
        {
            Customer customer = _unit.Customers.GetAll().Include(i => i.Invoices)
                .Include(r => r.Resources).FirstOrDefault(d => d.Id == id);
            return customer;
        }

        public Expense GetExpense(int id)
        {
            Expense expense = _unit.Expenses.GetAll().Include(cp => cp.Company).FirstOrDefault(d => d.Id == id);
            return expense;
        }

        public Company GetCompany()
        {
            Company customer = _unit.Companies.GetAll().Include(ex => ex.Expenses)
                 .Include(doc => doc.Documents).Include(rs => rs.Resources).FirstOrDefault(a => a.Id > 0);
            return customer;
        }

        public List<Resource> GetResourcesByType(string type)
        {
            var resources = _unit.Resources.GetAll().Where(d => d.Type == type).ToList();
            return resources;
        }

        public void AddResource(Resource resource)
        {
            _unit.Resources.Add(resource);
            _unit.SaveChanges();
        }

        public void CreateInvoice(int customerId, Invoice invoice)
        {
            Customer cust = this.GetCustomer(customerId);
            invoice.Customer = cust;
            _unit.Invoices.Add(invoice);
            try
            {
                _unit.SaveChanges();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public void CreateQoutation(Qoutation qoutation)
        {
            _unit.Qoutations.Add(qoutation);
            try
            {
                _unit.SaveChanges();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCompany(Company company)
        {
            _unit.Companies.Update(company);
            _unit.SaveChanges();
        }

        public List<BankStatement> GetBankStatement()
        {
            return _unit.BankStatements.GetAll().ToList();
        }

        public void AddBankStatment(BankStatement statement)
        {
            _unit.BankStatements.Add(statement);
            _unit.SaveChanges();
        }

        public List<Expense> GetExpenditures()
        {
            return _unit.Expenses.GetAll().ToList();
        }

        public void AddExpense(Expense expense)
        {
            _unit.Expenses.Add(expense);
            _unit.SaveChanges();
        }

        public int GetInvoiceCount()
        {
            return _unit.Invoices.GetAll().ToList().Count + 1;
        }
        public int GetQouteCount()
        {
            return _unit.Qoutations.GetAll().ToList().Count + 1;
        }

        public int GetExpenseCount()
        {

            return _unit.Expenses.GetAll().ToList().Count + 1;
        }

        public Invoice GetInvoice(int id)
        {

            return _unit.Invoices.GetAll().FirstOrDefault(inv => inv.Id == id);
        }

        public Qoutation GetQoutation(int id)
        {

            return _unit.Qoutations.GetAll().FirstOrDefault(qt => qt.Id == id);
        }


        public CompanyDocument GetCompanyDocument(int id)
        {

            return _unit.CompanyDocuments.GetAll().FirstOrDefault(inv => inv.Id == id);
        }

        public void UpdateInvoice(Invoice invoice)
        {
            _unit.Invoices.Update(invoice);
            _unit.SaveChanges();
        }

        public void UpdateCustomer(Customer customer)
        {
            _unit.Customers.Update(customer);
            _unit.SaveChanges();
        }

        public void DeleteExpenditure(int p)
        {
            _unit.Expenses.Delete(p);
            _unit.SaveChanges();
        }

        public void UpdateExpense(Expense expense)
        {
            _unit.Expenses.Update(expense);
            _unit.SaveChanges();
        }

        public BankStatement GetBankStatement(int id)
        {
            return _unit.BankStatements.GetAll().FirstOrDefault(inv => inv.Id == id);
        }

        public void UpdateBankStatement(BankStatement statement)
        {
            _unit.BankStatements.Update(statement);
            _unit.SaveChanges();
        }

        public List<Expense> GetAllExpense()
        {
            return _unit.Expenses.GetAll().ToList();
        }

        public List<Invoice> GetAllInvoice(string p = "")
        {
            if (p == string.Empty)
            {
                return _unit.Invoices.GetAll().Include(cs => cs.Customer).ToList();
            }
            return _unit.Invoices.GetAll().Include(cs => cs.Customer).Where(iv => iv.Status == p).ToList();
        }

        public void UpdateCompanyDocument(CompanyDocument selectedDocument)
        {
            _unit.CompanyDocuments.Update(selectedDocument);
            _unit.SaveChanges();
        }

        public void Dispose()
        {
            if (_unit != null)
            {
                _unit.Dispose();
            }
        }

        internal List<Qoutation> GetQoutations(int id)
        {
            return _unit.Qoutations.GetAll().Where(iv => iv.Customer_Id == id).ToList();
        }

        internal List<Qoutation> GetMoreQoute()
        {
            return _unit.Qoutations.GetAll().Where(iv => iv.Customer_Id == 0).ToList();
        }

        internal void CreateOnceOffInvoice(OnceOffInvoice invoice)
        {
            _unit.OnceOffInvoices.Add(invoice);
            try
            {
                _unit.SaveChanges();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        internal List<OnceOffInvoice> GetOnceOffInvoice()
        {
            return _unit.OnceOffInvoices.GetAll().ToList();
        }
    }
}