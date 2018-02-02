using System.Data.Entity.ModelConfiguration;
using IntroductionMVC5.Models.PettyCash;

namespace IntroductionMVC5.Data.Configuration
{
    public class PaymentTypeConfiguration : EntityTypeConfiguration<PaymentType>
    {
        public PaymentTypeConfiguration()
        {
            HasKey(pt => pt.Id);
        }
    }
}