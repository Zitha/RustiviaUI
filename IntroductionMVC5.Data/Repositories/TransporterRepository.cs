using System.Data.Entity;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Data.Repositories
{
    internal class TransporterRepository : GenericRepository<Transporter>
    {
        public TransporterRepository(DbContext context)
            : base(context)
        {
        }
    }
}