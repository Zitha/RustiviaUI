using IntroductionMVC5.Models.ArsloTrading;
using System.Data.Entity.ModelConfiguration;

namespace IntroductionMVC5.Data.Configuration.ArsloTrading
{
    public class ArsloProfomaConfiguration : EntityTypeConfiguration<ArsloProfoma>
    {
        public ArsloProfomaConfiguration()
        {
            HasKey(a => a.Id);
        }
    }
}
