using System.Data.Entity;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Data.Repositories
{
    public class WeighBridgeInfoRepository : GenericRepository<WeighBridgeInfo>
    {
        public WeighBridgeInfoRepository(DbContext context)
            : base(context)
        {
        }
    }
}