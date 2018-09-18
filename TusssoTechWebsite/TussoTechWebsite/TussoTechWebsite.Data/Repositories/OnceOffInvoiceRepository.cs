using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TussoTechWebsite.Model;

namespace TussoTechWebsite.Data.Repositories
{
    public class OnceOffInvoiceRepository : GenericRepository<OnceOffInvoice>
    {
        public OnceOffInvoiceRepository(DbContext context)
            : base(context)
        {
        }
    }
}
