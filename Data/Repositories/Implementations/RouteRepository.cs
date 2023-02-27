using Data.Entities;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations
{
    public class RouteRepository : Repository<Route>, IRouteRepository
    {
        public RouteRepository(CarRentalContext context) : base(context)
        {
        }
    }
}
