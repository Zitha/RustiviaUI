using System.Data.Entity;
using IntroductionMVC5.Models;

namespace IntroductionMVC5.Data.Repositories
{
    public class PastelInfoRepository : GenericRepository<PastelInfo>
    {
        public PastelInfoRepository(DbContext context)
            : base(context)
        {
        }
    }
}