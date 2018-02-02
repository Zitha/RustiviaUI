using System.Data.Entity.ModelConfiguration;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Data.Configuration
{
    public class SupplierInfoConfiguration : EntityTypeConfiguration<SupplierInfo>
    {
        public SupplierInfoConfiguration()
        {
            HasKey(d => d.Id);

            Property(p => p.SupplierName)
                .IsRequired().HasMaxLength(50);
        }
    }
}