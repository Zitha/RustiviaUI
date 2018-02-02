using System.Data.Entity;
using IntroductionMVC5.Models.PettyCash;

namespace IntroductionMVC5.Data.Repositories
{
    public class AccountRepository : GenericRepository<Account>
    {
        public AccountRepository(DbContext context)
            : base(context)
        {
        }
    }
}