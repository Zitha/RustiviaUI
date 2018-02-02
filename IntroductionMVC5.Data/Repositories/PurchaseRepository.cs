using System.Data.Entity;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Data.Repositories
{
    public class PurchaseRepository : GenericRepository<Purchase>
    {
        public PurchaseRepository(DbContext context) : base(context)
        {
        }
    }
}