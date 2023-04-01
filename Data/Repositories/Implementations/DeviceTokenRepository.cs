using Data.Entities;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations
{
    public class DeviceTokenRepository : Repository<DeviceToken>, IDeviceTokenRepository
    {
        public DeviceTokenRepository(CarRentalContext context) : base(context)
        {
        }
    }
}
