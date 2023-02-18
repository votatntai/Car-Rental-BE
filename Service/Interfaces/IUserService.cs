using Data.Models.Create;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface IUserService
    {
        Task<UserViewModel> GetUser(Guid id);
        Task<UserViewModel> CreateManager(UserCreateModel model);
    }
}
