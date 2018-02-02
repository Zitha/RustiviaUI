using System.Data.Entity;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Data.Repositories
{
    public class BankAccountRepository : GenericRepository<BankAccount>
    {
        public BankAccountRepository(DbContext context)
            : base(context)
        {
        }
    }
}