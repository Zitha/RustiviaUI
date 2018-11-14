using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TussoTechWebsite.Model;

namespace TussoTechWebsite.Data.Repositories
{
    public class CompanyRepository : GenericRepository<Company>
    {
        public CompanyRepository(DbContext context)
            : base(context)
        {
        }
    }
}
