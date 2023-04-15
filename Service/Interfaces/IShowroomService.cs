using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface IShowroomService
    {
        Task<ListViewModel<ShowroomViewModel>> GetShowrooms(ShowroomFilterModel filter, PaginationRequestModel pagination);
        Task<ShowroomViewModel> GetShowroom(Guid id);
        Task<ShowroomViewModel> CreateShowroom(ShowroomCreateModel model);
        Task<ShowroomViewModel> UpdateShowroom(Guid id, ShowroomUpdateModel model);
        Task<bool> DeleteShowroom(Guid id);
    }
}
