using Data.Models.Get;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface IAuthService
    {
        Task<AuthViewModel> AuthenticatedUser(AuthRequestModel model);
        Task<AuthViewModel> GetUserById(Guid id);
    }
}
