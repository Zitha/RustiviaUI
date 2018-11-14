using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TussoTechWebsite.Model;

namespace TussoTechWebsite.Data.Repositories
{
    public class QoutationRepository : GenericRepository<Qoutation>
    {
        public QoutationRepository(DbContext context)
            : base(context)
        {
        }
    }
}
