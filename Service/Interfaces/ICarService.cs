using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface ICarService
    {
        Task<ListViewModel<CarViewModel>> GetCars(CarFilterModel filter, PaginationRequestModel pagination);
        Task<CarViewModel> GetCar(Guid id);
        Task<CarViewModel> CreateCar(CarCreateModel model);
        Task<CarViewModel> UpdateCar(Guid id, CarUpdateModel model);
    }
}
