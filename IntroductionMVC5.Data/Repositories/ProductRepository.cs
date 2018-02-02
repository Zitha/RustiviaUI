using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Data.Repositories
{
    public class ProductRepository : GenericRepository<Product>
    {
        public ProductRepository(DbContext context)
            : base(context)
        {
        }
    }
}
