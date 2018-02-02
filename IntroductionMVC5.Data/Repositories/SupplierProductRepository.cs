using System.Data.Entity;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Data.Repositories
{
    public class SupplierProductRepository : GenericRepository<SupplierProduct>
    {
        public SupplierProductRepository(DbContext context)
            : base(context)
        {
        }
    }
}