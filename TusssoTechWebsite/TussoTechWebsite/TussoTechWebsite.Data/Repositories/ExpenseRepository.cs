using System.Data.Entity;
using TussoTechWebsite.Model;

namespace TussoTechWebsite.Data.Repositories
{
    public class ExpenseRepository : GenericRepository<Expense>
    {
        public ExpenseRepository(DbContext context)
            : base(context)
        {
        }
    }
}