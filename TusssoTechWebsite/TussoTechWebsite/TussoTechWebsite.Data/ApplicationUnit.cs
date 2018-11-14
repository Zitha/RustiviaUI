using System;
using System.Data.Entity.Validation;
using TussoTechWebsite.Data.Repositories;
using TussoTechWebsite.Model;

namespace TussoTechWebsite.Data
{
    public class ApplicationUnit : IDisposable
    {
        private readonly DataContext _context = new DataContext();
        private IRepository<BankStatement> _bankStatement;
        private IRepository<Company> _companies;
        private IRepository<CompanyDocument> _companyDocument;
        private IRepository<Customer> _customer;
        private IRepository<Expense> _expense;
        private IRepository<Invoice> _invoice;
        private IRepository<OnceOffInvoice> _onceOffInvoice;
        private IRepository<Resource> _resource;
        private IRepository<Item> _item;
        private IRepository<Qoutation> _qoutation;

        public IRepository<Customer> Customers
        {
            get
            {
                if (_customer == null)
                {
                    _customer = new CustomerRepository(_context);
                }
                return _customer;
            }
        }

        public IRepository<Invoice> Invoices
        {
            get
            {
                if (_invoice == null)
                {
                    _invoice = new InvoiceRepository(_context);
                }
                return _invoice;
            }
        }

        public IRepository<OnceOffInvoice> OnceOffInvoices
        {
            get
            {
                if (_onceOffInvoice == null)
                {
                    _onceOffInvoice = new OnceOffInvoiceRepository(_context);
                }
                return _onceOffInvoice;
            }
        }

        public IRepository<Expense> Expenses
        {
            get
            {
                if (_expense == null)
                {
                    _expense = new ExpenseRepository(_context);
                }
                return _expense;
            }
        }

        public IRepository<BankStatement> BankStatements
        {
            get
            {
                if (_bankStatement == null)
                {
                    _bankStatement = new BankStatementRepository(_context);
                }
                return _bankStatement;
            }
        }

        public IRepository<Resource> Resources
        {
            get
            {
                if (_resource == null)
                {
                    _resource = new ResourceRepository(_context);
                }
                return _resource;
            }
        }

        public IRepository<Company> Companies
        {
            get
            {
                if (_companies == null)
                {
                    _companies = new CompanyRepository(_context);
                }
                return _companies;
            }
        }

        public IRepository<Item> Items
        {
            get
            {
                if (_item == null)
                {
                    _item = new ItemRepository(_context);
                }
                return _item;
            }
        }

        public IRepository<CompanyDocument> CompanyDocuments
        {
            get
            {
                if (_companyDocument == null)
                {
                    _companyDocument = new CompanyDocumentRepository(_context);
                }
                return _companyDocument;
            }
        }

        public IRepository<Qoutation> Qoutations
        {
            get
            {
                if (_qoutation == null)
                {
                    _qoutation = new QoutationRepository(_context);
                }
                return _qoutation;
            }
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
            GC.Collect();
        }

        public void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
            }
        }
    }
}