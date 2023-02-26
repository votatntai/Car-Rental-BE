using Data;
using Data.Entities;
using Data.Models.Get;
using Data.Models.Views;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Service.Implementations
{
    public class CarService : BaseService, ICarService
    {
        private readonly ICarRepository _carRepository;
        public CarService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _carRepository = unitOfWork.Car;
        }

        public async Task<ListViewModel<CarViewModel>> GetCars(CarFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _carRepository.GetMany(car => car.Name != null && filter.Name != null ? car.Name.Contains(filter.Name) : true)
                .Select(car => new CarViewModel
                {
                    Id = car.Id,
                    Name = car.Name ?? null!,
                    CreateAt = car.CreateAt,
                    FuelType = car.FuelType,
                    FuelConsumption = car.FuelConsumption,
                    LicensePlate = car.LicensePlate,
                    Price = car.Price,
                    Rented = car.Rented,
                    Seater = car.Seater,
                    Star = car.Star ?? 0,
                    Status = car.Status,
                    TransmissionType = car.TransmissionType,
                    YearOfManufacture = car.YearOfManufacture,
                    Description = car.Description ?? null!,
                    Location = car.Location != null ? new LocationViewModel
                    {
                        Id = car.Location.Id,
                        Latitude = car.Location.Latitude,
                        Longitude = car.Location.Longitude
                    } : null!,
                    ProductionCompany = new ProductionCompanyViewModel
                    {
                        Id = car.ProductionCompany.Id,
                        Name = car.ProductionCompany.Name,
                        Description = car.ProductionCompany.Description ?? null!,
                    },
                    Route = car.Route != null ? new RouteViewModel
                    {
                        Id = car.Route.Id,
                        DistanceSurcharge = car.Route.DistanceSurcharge,
                        MaximumDistance = car.Route.MaximumDistance,
                        TimeSurcharge = car.Route.TimeSurcharge
                    } : null!
                });
            var cars = await query.Skip(pagination.PageNumber * pagination.PageSize).Take(pagination.PageSize).AsNoTracking().ToListAsync();
            var totalRow = await query.AsNoTracking().CountAsync();
            if(cars != null || cars != null && cars.Any())
            {
                return new ListViewModel<CarViewModel>
                {
                    Pagination = new PaginationViewModel
                    {
                        PageNumber = pagination.PageNumber,
                        PageSize = pagination.PageSize,
                        TotalRow = totalRow
                    },
                    Data = cars
                };
            }
            return null!;
        }
    }
}
