using System.Data.Entity;
using TussoTechWebsite.Model;

namespace TussoTechWebsite.Data.Repositories
{
    public class ResourceRepository : GenericRepository<Resource>
    {
        public ResourceRepository(DbContext context)
            : base(context)
        {
        }
    }
}