using System.Data.Entity.ModelConfiguration;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Data.Configuration
{
    public class TruckConfiguration : EntityTypeConfiguration<Truck>
    {
        public TruckConfiguration()
        {
            HasKey(d => d.Id);

            Property(p => p.TruckRegNumber)
                .IsRequired().HasMaxLength(15);
        }
    }
}