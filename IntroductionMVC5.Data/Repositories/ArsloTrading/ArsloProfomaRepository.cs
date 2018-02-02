using IntroductionMVC5.Models.ArsloTrading;
using System.Data.Entity;

namespace IntroductionMVC5.Data.Repositories.ArsloTrading
{
    public class ArsloProfomaRepository : GenericRepository<ArsloProfoma>
    {
        public ArsloProfomaRepository(DbContext context)
            : base(context)
        {
        }
    }
}
