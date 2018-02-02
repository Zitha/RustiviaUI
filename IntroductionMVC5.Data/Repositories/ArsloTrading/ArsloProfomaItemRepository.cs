using IntroductionMVC5.Models.ArsloTrading;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroductionMVC5.Data.Repositories.ArsloTrading
{
    public class ArsloProfomaItemRepository : GenericRepository<ArsloProfomaItem>
    {
        public ArsloProfomaItemRepository(DbContext context)
            : base(context)
        {
        }
    }
}
