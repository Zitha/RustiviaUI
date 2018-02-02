using System.Data.Entity.ModelConfiguration;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Data.Configuration
{
    public class BookingConfiguration : EntityTypeConfiguration<Booking>
    {
        public BookingConfiguration()
        {
            HasKey(d => d.Reference);

            Property(p => p.DateIn)
                .IsRequired();
            Property(p => p.DateOut)
                .IsOptional();
        }
    }
}