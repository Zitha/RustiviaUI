using System.Data.Entity.ModelConfiguration;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Data.Configuration
{
    public class TransporterConfiguration : EntityTypeConfiguration<Transporter>
    {
        public TransporterConfiguration()
        {
            HasKey(d => d.Id);

            Property(p => p.Name)
                .IsRequired().HasMaxLength(50);
        }
    }
}