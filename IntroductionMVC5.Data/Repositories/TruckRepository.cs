using System.Data.Entity;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Data.Repositories
{
    internal class TruckRepository : GenericRepository<Truck>
    {
        public TruckRepository(DbContext context) : base(context)
        {
        }
    }
}