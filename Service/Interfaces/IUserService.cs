using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface IUserService
    {
        Task<ListViewModel<UserViewModel>> GetUsers(UserFilterModel filter, PaginationRequestModel pagination);
        Task<UserViewModel> GetUser(Guid id);
        Task<UserViewModel> CreateManager(UserCreateModel model);
        Task<UserViewModel> UpdateUser(Guid id, UserUpdateModel model);
    }
}
