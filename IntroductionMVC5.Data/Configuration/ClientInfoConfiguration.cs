using System.Data.Entity.ModelConfiguration;
using RustiviaSolutions.Models;

namespace RustiviaSolutions.Data.Configuration
{
    public class ClientInfoConfiguration : EntityTypeConfiguration<ClientInfo>
    {
        public ClientInfoConfiguration()
        {
            HasKey(d => d.Id);

            Property(p => p.ClientName)
                .IsRequired().HasMaxLength(30);
        }
    }
}