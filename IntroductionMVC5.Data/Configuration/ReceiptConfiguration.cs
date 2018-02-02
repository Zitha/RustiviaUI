using System.Data.Entity.ModelConfiguration;
using IntroductionMVC5.Models.PettyCash;

namespace IntroductionMVC5.Data.Configuration
{
    public class ReceiptConfiguration : EntityTypeConfiguration<Receipt>
    {
        public ReceiptConfiguration()
        {
            HasKey(r => r.Id);

            Property(r => r.Reference)
                .IsRequired();

            Property(r => r.ExtraInfo)
                .IsOptional();
        }
    }
}