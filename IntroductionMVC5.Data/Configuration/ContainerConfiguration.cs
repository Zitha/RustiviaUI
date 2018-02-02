using System.Data.Entity.ModelConfiguration;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Data.Configuration
{
    public class ContainerConfiguration : EntityTypeConfiguration<Container>
    {
        public ContainerConfiguration()
        {
            HasKey(d => d.Id);

            Property(p => p.DateIn)
                .IsRequired();

            Property(p => p.DateIn)
                .IsRequired();

            HasOptional(w => w.WeighBridgeInfo)
                .WithOptionalDependent();
        }
    }
}