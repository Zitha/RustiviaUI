using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Migrations.History;
using MySql.Data.Entity;
using TussoTechWebsite.Data.Configuration;
using TussoTechWebsite.Model;

namespace TussoTechWebsite.Data
{
    //[DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class DataContext : DbContext
    {
        static DataContext()
        {
            //DbConfiguration.SetConfiguration(new MySqlEFConfiguration());
            Database.SetInitializer(new CustomDatabaseInitializer());
        }

        public DataContext()
            : base(ConnectionStringName)
        {
        }

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

        public DbSet<Customer> Customers
        {
            get;
            set;
        }
        public DbSet<Expense> Expenses
        {
            get;
            set;
        }
        public DbSet<Invoice> Invoices
        {
            get;
            set;
        }
        public DbSet<OnceOffInvoice> OnceOffInvoices
        {
            get;
            set;
        }
        public DbSet<Resource> Resources
        {
            get;
            set;
        }
        public DbSet<Company> Companies
        {
            get;
            set;
        }
        public DbSet<Item> Items
        {
            get;
            set;
        }

        public DbSet<CompanyDocument> CompanyDocuments
        {
            get;
            set;
        }
        public DbSet<BankStatement> BankStatementConfigurations
        {
            get;
            set;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CustomerConfiguration());
            modelBuilder.Configurations.Add(new ExpenseConfiguration());
            modelBuilder.Configurations.Add(new InvoiceConfiguration());
            modelBuilder.Configurations.Add(new OnceOffInvoiceConfiguration());
            modelBuilder.Configurations.Add(new ResourceConfiguration());
            modelBuilder.Configurations.Add(new CompanyConfiguration());
            modelBuilder.Configurations.Add(new CompanyDocumentConfiguration());
            modelBuilder.Configurations.Add(new BankStatementConfiguration());
            modelBuilder.Configurations.Add(new ItemConfiguration());
            modelBuilder.Configurations.Add(new QoutationConfiguration());

            //modelBuilder.Entity<HistoryRow>().Property(h => h.MigrationId).HasMaxLength(100).IsRequired();
            //modelBuilder.Entity<HistoryRow>().Property(h => h.ContextKey).HasMaxLength(500).IsRequired();
        }
    }
}