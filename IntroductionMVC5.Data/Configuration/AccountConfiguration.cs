using System.Data.Entity.ModelConfiguration;
using IntroductionMVC5.Models.PettyCash;

namespace IntroductionMVC5.Data.Configuration
{
    public class AccountConfiguration : EntityTypeConfiguration<Account>
    {
        public AccountConfiguration()
        {
            HasKey(a => a.Id);
        }
    }
}