using Data.Models.Create;

namespace Service.Interfaces
{
    public interface IDeviceTokenService
    {
        Task<bool> CreateDeviceToken(Guid userId, DeviceTokenCreateModel model);
    }
}
