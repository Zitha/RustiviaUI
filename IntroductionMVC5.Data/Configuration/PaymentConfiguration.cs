using System.Data.Entity.ModelConfiguration;
using IntroductionMVC5.Models.PettyCash;

namespace IntroductionMVC5.Data.Configuration
{
    public class PaymentConfiguration : EntityTypeConfiguration<Payment>
    {
        public PaymentConfiguration()
        {
            HasKey(k => k.Id);
        }
    }
}