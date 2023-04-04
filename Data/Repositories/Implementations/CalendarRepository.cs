using Data.Entities;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations
{
    public class CalendarRepository : Repository<Calendar>, ICalendarRepository
    {
        public CalendarRepository(CarRentalContext context) : base(context)
        {
        }
    }
}
