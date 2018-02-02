using IntroductionMVC5.Models.ArsloTrading;
using System.Data.Entity;

namespace IntroductionMVC5.Data.Repositories.ArsloTrading
{
    public class ArsloCustomerRepository : GenericRepository<ArsloCustomer>
    {
        public ArsloCustomerRepository(DbContext context)
            : base(context)
        {
        }
    }
}
