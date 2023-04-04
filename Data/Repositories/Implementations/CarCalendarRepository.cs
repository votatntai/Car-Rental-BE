using Data.Entities;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations
{
    public class CarCalendarRepository : Repository<CarCalendar>, ICarCalendarRepository
    {
        public CarCalendarRepository(CarRentalContext context) : base(context)
        {
        }
    }
}
