using Data.Models.Get;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface IAuthService
    {
        Task<AuthViewModel> AuthById(Guid id);
        Task<TokenViewModel> AuthenticatedUser(AuthRequestModel model);
        Task<UserViewModel> GetUserById(Guid id);
        Task<TokenViewModel> AuthenticatedCustomer(AuthRequestModel model);
        Task<CustomerViewModel> GetCustomerById(Guid id);
        Task<TokenViewModel> AuthenticatedDriver(AuthRequestModel model);
        Task<DriverViewModel> GetDriverById(Guid id);
        Task<TokenViewModel> AuthenticatedCarOwner(AuthRequestModel model);
        Task<CarOwnerViewModel> GetCarOwnerById(Guid id);
    }
}
