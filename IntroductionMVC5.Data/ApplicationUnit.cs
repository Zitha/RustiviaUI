using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using IntroductionMVC5.Data.Repositories;
using IntroductionMVC5.Models;
using IntroductionMVC5.Models.Integrator;
using IntroductionMVC5.Models.PettyCash;
using IntroductionMVC5.Models.ArsloTrading;
using IntroductionMVC5.Data.Repositories.ArsloTrading;

namespace IntroductionMVC5.Data
{
    public class ApplicationUnit : IDisposable
    {
        private readonly DataContext _context = new DataContext();
        #region Rustivia Integration

        private IRepository<Account> _accounts;
        private IRepository<Booking> _bookings;
        private IRepository<Container> _containers;
        private IRepository<Customer> _customers;
        private IRepository<Driver> _drivers;
        private IRepository<PastelInfo> _pastelInfos;
        private IRepository<PaymentType> _paymentTypes;
        private IRepository<Payment> _payments;
        private IRepository<Purchase> _purchase;
        private IRepository<SupplierInfo> _supplierInfos;
        private IRepository<Transporter> _transporter;
        private IRepository<Truck> _truck;
        private IRepository<User> _users;
        private IRepository<WeighBridgeInfo> _weighBridgeInfos;
        private IRepository<Receipt> _receipt;
        private IRepository<EndDayBalance> _endDayBalance;
        private IRepository<Product> _product;
        private IRepository<SupplierProduct> _supplierProduct;
        private IRepository<Sale> _sales;
        private IRepository<BankAccount> _bankAccount;

        #endregion

        #region Arslo Trading

        private IRepository<ArsloCustomer> _arsloCustomer;
        private IRepository<ArsloInvoice> _arsloInvoice;
        private IRepository<ArsloInvoiceItem> _arsloInvoiceItem;
        private IRepository<ArsloProfoma> _arsloProfoma;
        private IRepository<ArsloProfomaItem> _arsloProfomaItem;

        #endregion

        #region Rustivia

        public IRepository<Sale> Sale
        {
            get
            {
                if (_sales == null)
                {
                    _sales = new SaleRepository(_context);
                }
                return _sales;
            }
        }

        public IRepository<SupplierProduct> SupplierProduct
        {
            get
            {
                if (_supplierProduct == null)
                {
                    _supplierProduct = new SupplierProductRepository(_context);
                }
                return _supplierProduct;
            }
        }

        public IRepository<Purchase> Purchase
        {
            get
            {
                if (_purchase == null)
                {
                    _purchase = new PurchaseRepository(_context);
                }
                return _purchase;
            }
        }

        public IRepository<Product> Product
        {
            get
            {
                if (_product == null)
                {
                    _product = new ProductRepository(_context);
                }
                return _product;
            }
        }

        public IRepository<Driver> Drivers
        {
            get
            {
                if (_drivers == null)
                {
                    _drivers = new DriversRepository(_context);
                }
                return _drivers;
            }
        }

        public IRepository<PastelInfo> PastelInfos
        {
            get
            {
                if (_pastelInfos == null)
                {
                    _pastelInfos = new PastelInfoRepository(_context);
                }
                return _pastelInfos;
            }
        }

        public IRepository<WeighBridgeInfo> WeighBridgeInfos
        {
            get
            {
                if (_weighBridgeInfos == null)
                {
                    _weighBridgeInfos = new WeighBridgeInfoRepository(_context);
                }
                return _weighBridgeInfos;
            }
        }

        public IRepository<SupplierInfo> SupplierInfo
        {
            get
            {
                if (_supplierInfos == null)
                {
                    _supplierInfos = new ClientInfoRepository(_context);
                }
                return _supplierInfos;
            }
        }

        public IRepository<Truck> Truck
        {
            get
            {
                if (_truck == null)
                {
                    _truck = new TruckRepository(_context);
                }
                return _truck;
            }
        }

        public IRepository<Transporter> Transporter
        {
            get
            {
                if (_transporter == null)
                {
                    _transporter = new TransporterRepository(_context);
                }
                return _transporter;
            }
        }

        public IRepository<User> Users
        {
            get
            {
                if (_users == null)
                {
                    _users = new UsersRepository(_context);
                }
                return _users;
            }
        }

        public IRepository<Customer> Customers
        {
            get
            {
                if (_customers == null)
                {
                    _customers = new CustomerRepository(_context);
                }
                return _customers;
            }
        }

        public IRepository<Booking> Bookings
        {
            get
            {
                if (_bookings == null)
                {
                    _bookings = new BookingRepository(_context);
                }
                return _bookings;
            }
        }

        public IRepository<Container> Containers
        {
            get
            {
                if (_containers == null)
                {
                    _containers = new ContainerRepository(_context);
                }
                return _containers;
            }
        }

        public IRepository<Account> Accounts
        {
            get
            {
                if (_accounts == null)
                {
                    _accounts = new AccountRepository(_context);
                }
                return _accounts;
            }
        }

        public IRepository<PaymentType> PaymentTypes
        {
            get
            {
                if (_paymentTypes == null)
                {
                    _paymentTypes = new PaymentTypeRepository(_context);
                }
                return _paymentTypes;
            }
        }

        public IRepository<Payment> Payments
        {
            get
            {
                if (_payments == null)
                {
                    _payments = new PaymentRepository(_context);
                }
                return _payments;
            }
        }

        public IRepository<Receipt> Receipts
        {
            get
            {
                if (_receipt == null)
                {
                    _receipt = new ReceiptRepository(_context);
                }
                return _receipt;
            }
        }

        public IRepository<EndDayBalance> EndDayBalances
        {
            get
            {
                if (_endDayBalance == null)
                {
                    _endDayBalance = new EndDayBalanceRepository(_context);
                }
                return _endDayBalance;
            }
        }

        public IRepository<BankAccount> BankAccounts
        {
            get
            {
                if (_bankAccount == null)
                {
                    _bankAccount = new BankAccountRepository(_context);
                }
                return _bankAccount;
            }
        }

        #endregion

        #region Arslo Trading
        public IRepository<ArsloCustomer> ArsloCustomers
        {
            get
            {
                if (_arsloCustomer == null)
                {
                    _arsloCustomer = new ArsloCustomerRepository(_context);
                }
                return _arsloCustomer;
            }
        }

        public IRepository<ArsloInvoice> ArsloInvoices
        {
            get
            {
                if (_arsloInvoice == null)
                {
                    _arsloInvoice = new ArsloInvoiceRepository(_context);
                }
                return _arsloInvoice;
            }
        }

        public IRepository<ArsloInvoiceItem> ArsloInvoiceItems
        {
            get
            {
                if (_arsloInvoiceItem == null)
                {
                    _arsloInvoiceItem = new ArsloInvoiceItemRepository(_context);
                }
                return _arsloInvoiceItem;
            }
        }

        public IRepository<ArsloProfoma> ArsloProfomas
        {
            get
            {
                if (_arsloProfoma == null)
                {
                    _arsloProfoma = new ArsloProfomaRepository(_context);
                }
                return _arsloProfoma;
            }
        }

        public IRepository<ArsloProfomaItem> ArsloProfomaItems
        {
            get
            {
                if (_arsloProfomaItem == null)
                {
                    _arsloProfomaItem = new ArsloProfomaItemRepository(_context);
                }
                return _arsloProfomaItem;
            }
        }
        #endregion

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                var outputLines = new List<string>();
                foreach (DbEntityValidationResult eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format(
                        "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:",
                        DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (DbValidationError ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format(
                            "- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage));
                    }
                }
                File.AppendAllLines(@"c:\temp\errors.txt", outputLines);
                throw;
            }
        }
    }
}