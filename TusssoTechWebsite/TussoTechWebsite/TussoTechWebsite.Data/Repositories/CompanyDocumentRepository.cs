using System.Data.Entity;
using TussoTechWebsite.Model;

namespace TussoTechWebsite.Data.Repositories
{
    public class CompanyDocumentRepository : GenericRepository<CompanyDocument>
    {
        public CompanyDocumentRepository(DbContext context)
            : base(context)
        {
        }
    }
}