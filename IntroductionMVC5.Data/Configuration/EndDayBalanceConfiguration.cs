using System.Data.Entity.ModelConfiguration;
using IntroductionMVC5.Models.PettyCash;

namespace IntroductionMVC5.Data.Configuration
{
    public class EndDayBalanceConfiguration : EntityTypeConfiguration<EndDayBalance>
    {
        public EndDayBalanceConfiguration()
        {
            HasKey(k => k.Id);
        }
    }
}