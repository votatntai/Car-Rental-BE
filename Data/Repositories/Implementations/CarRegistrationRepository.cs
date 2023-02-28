using Data.Entities;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations
{
    public class CarRegistrationRepository : Repository<CarRegistration>, ICarRegistrationRepository
    {
        public CarRegistrationRepository(CarRentalContext context) : base(context)
        {
        }
    }
}
