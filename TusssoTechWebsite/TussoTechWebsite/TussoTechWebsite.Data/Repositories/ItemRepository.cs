using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TussoTechWebsite.Model;

namespace TussoTechWebsite.Data.Repositories
{
    public class ItemRepository : GenericRepository<Item>
    {
        public ItemRepository(DbContext context)
            : base(context)
        {
        }
    }
}
