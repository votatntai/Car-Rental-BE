using Data;
using Data.Entities;
using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;
using Data.Repositories.Interfaces;
using Extensions.MyExtentions;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using Utility.Constants;

namespace Service.Implementations
{
    public class CarService : BaseService, ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IRouteRepository _routeRepository;

        public CarService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _carRepository = unitOfWork.Car;
            _locationRepository = unitOfWork.Location;
            _routeRepository = unitOfWork.Route;
        }

        public async Task<ListViewModel<CarViewModel>> GetCars(CarFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _carRepository.GetMany(car => car.Name != null && filter.Name != null ? car.Name.Contains(filter.Name) : true);
            if (filter.Location != null)
            {
                query = query.AsQueryable().DistanceFilter(filter.Location.Latitude, filter.Location.Longitude, 'K');
            }
            var cars = await query.Skip(pagination.PageNumber * pagination.PageSize).Take(pagination.PageSize)
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
                }).ToListAsync();
            var totalRow = await query.CountAsync();
            if (cars != null || cars != null && cars.Any())
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

        public async Task<CarViewModel> GetCar(Guid id)
        {
            return await _carRepository.GetMany(car => car.Id.Equals(id)).Select(car => new CarViewModel
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
            }).FirstOrDefaultAsync() ?? null!;
        }

        public async Task<CarViewModel> CreateCar(CarCreateModel model)
        {
            var result = 0;
            var id = Guid.NewGuid();
            using (var transaction = _unitOfWork.Transaction())
            {
                try
                {
                    var locationId = await CreateLocation(model.Location);
                    var routeId = await CreateRoute(model.Route);

                    var car = new Car
                    {
                        Id = id,
                        FuelConsumption = model.FuelConsumption,
                        FuelType = model.FuelType,
                        Description = model.Description,
                        LocationId = locationId,
                        RouteId = routeId,
                        LicensePlate = model.LicensePlate,
                        Name = model.Name,
                        Price = model.Price,
                        Seater = model.Seater,
                        ProductionCompanyId = model.ProductionCompanyId,
                        TransmissionType = model.TransmissionType,
                        YearOfManufacture = model.YearOfManufacture,
                        Status = CarStatus.Idle.ToString(),
                        CreateAt = DateTime.Now
                    };
                    _carRepository.Add(car);
                    result = await _unitOfWork.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            return result > 0 ? await GetCar(id) : null!;
        }

        public async Task<CarViewModel> UpdateCar(Guid id, CarUpdateModel model)
        {
            var car = await _carRepository.GetMany(car => car.Id.Equals(id))
                .Include(car => car.Location)
                .Include(car => car.Route)
                .FirstOrDefaultAsync();
            if (car != null)
            {
                if (model.Name != null) car.Name = model.Name;
                if (model.Seater != null) car.Seater = (int)model.Seater;
                if (model.FuelType != null) car.FuelType = model.FuelType;
                if (model.FuelConsumption != null) car.FuelConsumption = model.FuelConsumption;
                if (model.Description != null) car.Description = model.Description;
                if (model.Status != null) car.Status = model.Status;
                if (model.LicensePlate != null) car.LicensePlate = model.LicensePlate;
                if (model.TransmissionType != null) car.TransmissionType = model.TransmissionType;
                if (model.YearOfManufacture != null) car.YearOfManufacture = (int)model.YearOfManufacture;
                if (model.ProductionCompanyId != null) car.ProductionCompanyId = (Guid)model.ProductionCompanyId;
                if (model.Price != null) car.Price = (double)model.Price;
                if (model.Location != null)
                {
                    car.Location!.Longitude = model.Location.Longitude;
                    car.Location!.Latitude = model.Location.Latitude;
                }
                if (model.Route != null)
                {
                    car.Route!.TimeSurcharge = model.Route.TimeSurcharge;
                    car.Route!.MaximumDistance = model.Route.MaximumDistance;
                    car.Route!.DistanceSurcharge = model.Route.DistanceSurcharge;
                }
                if (model.LicensePlate != null) car.LicensePlate = model.LicensePlate;

                _carRepository.Update(car);
                var result = await _unitOfWork.SaveChanges();
                return await GetCar(id);
            }
            return null!;
        }

        // PRIVATE METHODS

        private async Task<Guid> CreateLocation(LocationCreateModel model)
        {
            var id = Guid.NewGuid();
            var location = new Location
            {
                Id = id,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
            };
            _locationRepository.Add(location);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? id : Guid.Empty;
        }

        private async Task<Guid> CreateRoute(RouteCreateModel model)
        {
            var id = Guid.NewGuid();
            var route = new Route
            {
                Id = id,
                DistanceSurcharge = model.DistanceSurcharge,
                MaximumDistance = model.MaximumDistance,
                TimeSurcharge = model.TimeSurcharge
            };
            _routeRepository.Add(route);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? id : Guid.Empty;
        }
    }
}
