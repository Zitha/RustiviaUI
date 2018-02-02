using System.Data.Entity.ModelConfiguration;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Data.Configuration
{
    public class BankAccountConfiguration : EntityTypeConfiguration<BankAccount>
    {
        public BankAccountConfiguration()
        {
            HasKey(a => a.Id);
        }
    }
}