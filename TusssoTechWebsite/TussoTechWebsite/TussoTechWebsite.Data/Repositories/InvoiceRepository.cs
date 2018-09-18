using System.Data.Entity;
using TussoTechWebsite.Model;

namespace TussoTechWebsite.Data.Repositories
{
    public class InvoiceRepository : GenericRepository<Invoice>
    {
        public InvoiceRepository(DbContext context)
            : base(context)
        {
        }
    }
}