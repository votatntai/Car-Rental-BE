using Data.Models.Create;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface ICarOwnerService
    {
        Task<CarOwnerViewModel> GetCarOwner(Guid id);
        Task<CarOwnerViewModel> CreateCarOwner(CarOwnerCreateModel model);
    }
}
