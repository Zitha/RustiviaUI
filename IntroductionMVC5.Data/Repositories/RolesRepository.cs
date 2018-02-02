using System.Data.Entity;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Data.Repositories
{
    public class RolesRepository : GenericRepository<Role>
    {
        public RolesRepository(DbContext context) : base(context)
        {
        }
    }
}