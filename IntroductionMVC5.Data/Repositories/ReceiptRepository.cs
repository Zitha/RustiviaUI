using System.Data.Entity;
using IntroductionMVC5.Models.PettyCash;

namespace IntroductionMVC5.Data.Repositories
{
    public class ReceiptRepository : GenericRepository<Receipt>
    {
        public ReceiptRepository(DbContext context)
            : base(context)
        {
        }
    }
}