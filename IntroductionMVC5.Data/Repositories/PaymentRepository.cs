using System.Data.Entity;
using IntroductionMVC5.Models.PettyCash;

namespace IntroductionMVC5.Data.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>
    {
        public PaymentRepository(DbContext context) : base(context)
        {
        }
    }
}