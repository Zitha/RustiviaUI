using System.Data.Entity.ModelConfiguration;
using IntroductionMVC5.Models;

namespace IntroductionMVC5.Data.Configuration
{
    public class PastelInfoConfiguration : EntityTypeConfiguration<PastelInfo>
    {
        public PastelInfoConfiguration()
        {
            HasKey(d => d.Id);
            Property(p => p.PastelNumber)
                .IsRequired();

            Property(p => p.Date)
                .IsOptional();

            Property(p => p.FileLocation)
                .IsRequired();
        }
    }
}