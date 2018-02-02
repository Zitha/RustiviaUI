using System.Data.Entity;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Data.Repositories
{
    public class SaleRepository : GenericRepository<Sale>
    {
        public SaleRepository(DbContext context)
            : base(context)
        {
        }
    }
}