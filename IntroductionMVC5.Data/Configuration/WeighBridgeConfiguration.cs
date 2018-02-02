using System.Data.Entity.ModelConfiguration;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Data.Configuration
{
    public class WeighBridgeConfiguration : EntityTypeConfiguration<WeighBridgeInfo>
    {
        public WeighBridgeConfiguration()
        {
            HasKey(d => d.Id);

            Property(p => p.NettMass)
                .IsRequired();

            Property(p => p.FirstMass)
                .IsOptional();

            Property(p => p.SecondMass)
                .IsOptional();

            Property(p => p.Comments)
                .IsOptional();

            Property(p => p.DateIn).IsRequired();
        }
    }
}