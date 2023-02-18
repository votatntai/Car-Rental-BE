using Data.Entities;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations
{
    public class CarOwnerRepository : Repository<CarOwner>, ICarOwnerRepository
    {
        public CarOwnerRepository(CarRentalContext context) : base(context)
        {
        }
    }
}
