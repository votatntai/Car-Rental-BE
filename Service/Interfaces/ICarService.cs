using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;
using Microsoft.AspNetCore.Http;
using Utility.Enums;

namespace Service.Interfaces
{
    public interface ICarService
    {
        Task<ListViewModel<CarViewModel>> GetCars(CarFilterModel filter, PaginationRequestModel pagination);
        Task<CarViewModel> GetCar(Guid id);
        Task<ICollection<CarCalendarViewModel>> GetCarCalendar(Guid id);
        Task<CarViewModel> CreateCar(CarCreateModel model);
        Task<CarViewModel> CreateShowroomCar(ICollection<IFormFile> images, ICollection<IFormFile> licenses, CarShowroomCreateModel model);
        Task<CarViewModel> UpdateCar(Guid id, CarUpdateModel model);
        Task<ListViewModel<CarViewModel>> GetCarsByCarOwnerId(Guid carOwnerId, CarFilterModel filter, PaginationRequestModel pagination);
        Task<ICollection<CarViewModel>> GetCarsIsNotTracking(Guid carOwnerId, PaginationRequestModel pagination);
        Task<CarViewModel> TrackingACar(Guid carId);
        Task<CarViewModel> CancelTrackingACar(Guid carId);
    }
}
