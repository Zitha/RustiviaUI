using System.Data.Entity;
using TussoTechWebsite.Model;

namespace TussoTechWebsite.Data.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>
    {
        public CustomerRepository(DbContext context)
            : base(context)
        {
        }
    }
}