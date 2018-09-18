using System.Data.Entity;
using TussoTechWebsite.Model;

namespace TussoTechWebsite.Data.Repositories
{
    public class BankStatementRepository : GenericRepository<BankStatement>
    {
        public BankStatementRepository(DbContext context)
            : base(context)
        {
        }
    }
}