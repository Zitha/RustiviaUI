using System.Data.Entity.ModelConfiguration;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Data.Configuration
{
    public class PurchaseConfiguration : EntityTypeConfiguration<Purchase>
    {
        public PurchaseConfiguration()
        {
            HasKey(d => d.Id);
        }
    }
}