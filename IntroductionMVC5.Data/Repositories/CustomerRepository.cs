using System.Data.Entity;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Data.Repositories
{
    internal class CustomerRepository : GenericRepository<Customer>
    {
        public CustomerRepository(DbContext context) : base(context)
        {
        }
    }
}