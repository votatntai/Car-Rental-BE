using Data.Entities;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations
{
    public class AdditionalChargeRepository : Repository<AdditionalCharge>, IAdditionalChargeRepository
    {
        public AdditionalChargeRepository(CarRentalContext context) : base(context)
        {
        }
    }
}
