using System.Data.Entity;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Data.Repositories
{
    public class ContainerRepository : GenericRepository<Container>
    {
        public ContainerRepository(DbContext context) : base(context)
        {
        }
    }
}