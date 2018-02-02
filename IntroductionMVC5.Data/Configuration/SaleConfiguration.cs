using System.Data.Entity.ModelConfiguration;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Data.Configuration
{
    public class SaleConfiguration : EntityTypeConfiguration<Sale>
    {
        public SaleConfiguration()
        {
            HasKey(d => d.Id);
        }
    }
}