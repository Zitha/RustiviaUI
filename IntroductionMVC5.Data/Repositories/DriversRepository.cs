using System.Data.Entity;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Data.Repositories
{
    public class DriversRepository : GenericRepository<Driver>
    {
        public DriversRepository(DbContext context) : base(context)
        {
        }
    }
}