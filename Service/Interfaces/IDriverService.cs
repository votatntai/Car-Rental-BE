using Data.Models.Create;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface IDriverService
    {
        Task<DriverViewModel> GetDriver(Guid id);
        Task<DriverViewModel> CreateDriver(DriverCreateModel model);
    }
}
