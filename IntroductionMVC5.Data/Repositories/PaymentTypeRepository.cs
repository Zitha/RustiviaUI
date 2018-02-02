using System.Data.Entity;
using IntroductionMVC5.Models.PettyCash;

namespace IntroductionMVC5.Data.Repositories
{
    public class PaymentTypeRepository : GenericRepository<PaymentType>
    {
        public PaymentTypeRepository(DbContext context) : base(context)
        {
        }
    }
}