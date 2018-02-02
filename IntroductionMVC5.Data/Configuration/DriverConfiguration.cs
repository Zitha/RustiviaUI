using System.Data.Entity.ModelConfiguration;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Data.Configuration
{
    public class DriverConfiguration : EntityTypeConfiguration<Driver>
    {
        public DriverConfiguration()
        {
            HasKey(d => d.Id);

            Property(p => p.IdNumber)
                .IsRequired().HasMaxLength(15);

            Property(p => p.IdLocation)
                .IsRequired();

            Property(p => p.Gender)
                .IsRequired();

            Property(p => p.Firstname).IsRequired()
                .HasMaxLength(20);
        }
    }
}