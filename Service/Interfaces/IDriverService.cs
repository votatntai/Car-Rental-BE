using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface IDriverService
    {
        Task<ListViewModel<DriverViewModel>> GetDrivers(DriverFilterModel filter, PaginationRequestModel pagination);
        Task<DriverViewModel> GetDriver(Guid id);
        Task<DriverViewModel> CreateDriver(DriverCreateModel model);
        Task<DriverViewModel> UpdateDriver(Guid id, DriverUpdateModel model);
    }
}
