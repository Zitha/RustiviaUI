using IntroductionMVC5.Models.ArsloTrading;
using System.Data.Entity.ModelConfiguration;

namespace IntroductionMVC5.Data.Configuration.ArsloTrading
{
    public class ArsloCustomerConfiguration : EntityTypeConfiguration<ArsloCustomer>
    {
        public ArsloCustomerConfiguration()
        {
            HasKey(a => a.Id);
        }
    }
}
