using System.Data.Entity;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Data.Repositories
{
    public class BookingRepository : GenericRepository<Booking>
    {
        public BookingRepository(DbContext context) : base(context)
        {
        }
    }
}