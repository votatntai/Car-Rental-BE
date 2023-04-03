using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface ICarRegistrationService
    {
        Task<ListViewModel<CarRegistrationViewModel>> GetCarRegistrations(CarRegistrationFilterModel filter, PaginationRequestModel pagination);
        Task<CarRegistrationViewModel> GetCarRegistration(Guid id);
        Task<CarRegistrationViewModel> CreateCarRegistration(Guid carOwnerId, CarRegistrationCreateModel model);
        Task<CarRegistrationViewModel> UpdateCar(Guid id, CarRegistrationUpdateModel model);
        Task<bool> DeleteCarRegistration(Guid id);
    }
}
