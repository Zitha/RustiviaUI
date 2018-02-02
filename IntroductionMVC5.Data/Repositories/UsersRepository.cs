using System.Data.Entity;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Data.Repositories
{
    public class UsersRepository : GenericRepository<User>
    {
        public UsersRepository(DbContext context) : base(context)
        {
        }
    }
}