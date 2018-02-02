using System.Data.Entity;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Data.Repositories
{
    public class ClientInfoRepository : GenericRepository<SupplierInfo>
    {
        public ClientInfoRepository(DbContext context)
            : base(context)
        {
        }
    }
}