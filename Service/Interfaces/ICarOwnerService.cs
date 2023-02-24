using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface ICarOwnerService
    {
        Task<ListViewModel<CarOwnerViewModel>> GetCarOwners(CarOwnerFilterModel filter, PaginationRequestModel pagination);
        Task<CarOwnerViewModel> GetCarOwner(Guid id);
        Task<CarOwnerViewModel> CreateCarOwner(CarOwnerCreateModel model);
        Task<CarOwnerViewModel> UpdateCarOwner(Guid id, CarOwnerUpdateModel model);
    }
}
