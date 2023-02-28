using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface ICarRegistrationService
    {
        Task<ListViewModel<CarRegistrationViewModel>> GetCarRegistrations(CarRegistrationFilterModel filter, PaginationRequestModel pagination);
        Task<CarRegistrationViewModel> GetCarRegistration(Guid id);
        Task<CarRegistrationViewModel> CreateCarRegistration(CarRegistrationCreateModel model);
        Task<bool> DeleteCarRegistration(Guid id);
    }
}
