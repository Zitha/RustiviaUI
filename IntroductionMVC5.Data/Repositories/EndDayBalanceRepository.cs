using System.Data.Entity;
using IntroductionMVC5.Models.PettyCash;

namespace IntroductionMVC5.Data.Repositories
{
    public class EndDayBalanceRepository : GenericRepository<EndDayBalance>
    {
        public EndDayBalanceRepository(DbContext context)
            : base(context)
        {
        }
    }
}