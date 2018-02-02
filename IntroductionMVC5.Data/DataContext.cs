using System.Configuration;
using System.Data.Entity;
using IntroductionMVC5.Data.Configuration;
using IntroductionMVC5.Models;
using IntroductionMVC5.Models.Integrator;
using IntroductionMVC5.Data.Configuration.ArsloTrading;

namespace IntroductionMVC5.Data
{
    public class DataContext : DbContext
    {
        static DataContext()
        {
            Database.SetInitializer(new CustomDatabaseInitializer());
        }

        public DataContext()
            : base(ConnectionStringName)
        {
        }

        public DbSet<Driver> Drivers { get; set; }
        public DbSet<PastelInfo> PastelInfos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SupplierInfo> SupplierInfos { get; set; }
        public DbSet<WeighBridgeInfo> WeighBridgeInfos { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        public static string ConnectionStringName
        {
            get
            {
                if (ConfigurationManager.AppSettings["ConnectionStringName"]
                    != null)
                {
                    return ConfigurationManager.
                        AppSettings["ConnectionStringName"];
                }

                return "DefaultConnection";
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region Rustivia Integration

            modelBuilder.Configurations.Add(new DriverConfiguration());
            modelBuilder.Configurations.Add(new WeighBridgeConfiguration());
            modelBuilder.Configurations.Add(new SupplierInfoConfiguration());
            modelBuilder.Configurations.Add(new PastelInfoConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new CustomerConfiguration());
            modelBuilder.Configurations.Add(new BookingConfiguration());
            modelBuilder.Configurations.Add(new ContainerConfiguration());
            modelBuilder.Configurations.Add(new PurchaseConfiguration());
            modelBuilder.Configurations.Add(new TransporterConfiguration());
            modelBuilder.Configurations.Add(new TruckConfiguration());
            modelBuilder.Configurations.Add(new AccountConfiguration());
            modelBuilder.Configurations.Add(new PaymentConfiguration());
            modelBuilder.Configurations.Add(new EndDayBalanceConfiguration());
            modelBuilder.Configurations.Add(new PaymentTypeConfiguration());
            modelBuilder.Configurations.Add(new ReceiptConfiguration());
            modelBuilder.Configurations.Add(new ProductConfiguration());
            modelBuilder.Configurations.Add(new SupplierProductConfiguration());
            modelBuilder.Configurations.Add(new SaleConfiguration());
            modelBuilder.Configurations.Add(new BankAccountConfiguration());

            #endregion
            // Add ASP.NET WebPages SimpleSecurity tables
            modelBuilder.Configurations.Add(new RoleConfiguration());

            #region Arslo Trading
            modelBuilder.Configurations.Add(new ArsloCustomerConfiguration());
            modelBuilder.Configurations.Add(new ArsloInvoiceConfiguration());
            modelBuilder.Configurations.Add(new ArsloInvoiceItemConfiguration());
            modelBuilder.Configurations.Add(new ArsloProfomaConfiguration());
            modelBuilder.Configurations.Add(new ArsloProfomaItemConfiguration());
            #endregion
            //base.OnModelCreating(modelBuilder);
        }
    }
}