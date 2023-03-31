using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
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
using Utility.Enums;

namespace Service.Implementations
{
    public class CarService : BaseService, ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IAdditionalChargeRepository _additionalChargeRepository;
        private new readonly IMapper _mapper;

        public CarService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _carRepository = unitOfWork.Car;
            _locationRepository = unitOfWork.Location;
            _additionalChargeRepository = unitOfWork.AdditionalCharge;
            _mapper = mapper;
        }

        public async Task<ListViewModel<CarViewModel>> GetCars(CarFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _carRepository.GetMany(car => car.Name != null && filter.Name != null ? car.Name.Contains(filter.Name) : true);
            if (filter.Location != null)
            {
                query = query.AsQueryable().DistanceFilter(filter.Location.Latitude, filter.Location.Longitude);
            }
            if (filter.CarType != null)
            {
                query = query.AsQueryable().Where(car => car.CarTypes.Any(carType => carType.Type.Name.Equals(filter.CarType.ToString())));
            }
            if (filter.ModelId != null)
            {
                query = query.AsQueryable().Where(car => car.ModelId.Equals(filter.ModelId));
            }
            if (filter.Price != null)
            {
                query = query.AsQueryable().Where(car => car.Price >= filter.Price.MinPrice && car.Price <= filter.Price.MaxPrice);
            }
            if (filter.HasDriver != null)
            {
                query.AsQueryable().Where(car => car.DriverId != null);
            }
            if (filter.TransmissionType != null)
            {
                query = query.AsQueryable().Where(car => car.Model.TransmissionType.Equals(filter.TransmissionType.ToString()));
            }
            if (filter.StartTime != null && filter.EndTime != null)
            {
                var startTime = new TimeSpan(filter.StartTime.Value.Hour, filter.StartTime.Value.Minute, filter.StartTime.Value.Second);
                var endTime = new TimeSpan(filter.EndTime.Value.Hour, filter.StartTime.Value.Minute, filter.StartTime.Value.Second);
                query = query.AsQueryable().Where(car => car.ReceiveStartTime <= startTime && car.ReceiveEndTime >= endTime);
            }
            var cars = await query
            .Include(car => car.Model).ThenInclude(model => model.ProductionCompany)
            .OrderByDescending(car => car.Star)
            .ThenByDescending(car => car.Rented)
            .ProjectTo<CarViewModel>(_mapper.ConfigurationProvider)
            .Skip(pagination.PageNumber * pagination.PageSize).Take(pagination.PageSize)
            .ToListAsync();
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
            return await _carRepository.GetMany(car => car.Id.Equals(id))
                .Include(car => car.Model).ThenInclude(model => model.ProductionCompany)
                .ProjectTo<CarViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? null!;
        }

        public async Task<CarViewModel> CreateCar(CarCreateModel model)
        {
            using var transaction = _unitOfWork.Transaction();
            try
            {
                var locationId = await CreateLocation(model.Location);
                var additionalChargeId = await CreateAdditionalCharge(model.AdditionalCharge);

                var car = new Car
                {
                    Id = Guid.NewGuid(),
                    Description = model.Description,
                    LocationId = locationId,
                    AdditionalChargeId = additionalChargeId,
                    LicensePlate = model.LicensePlate,
                    Name = model.Name,
                    Price = model.Price,
                    Status = CarStatus.Idle.ToString(),
                    ModelId = model.ModelId,
                    ReceiveStartTime = model.ReceiveStartTime,
                    ReceiveEndTime = model.ReceiveEndTime,
                    ReturnStartTime = model.ReturnStartTime,
                    ReturnEndTime = model.ReturnEndTime,
                    Rented = 0,
                    CreateAt = DateTime.Now
                };
                _carRepository.Add(car);

                if (await _unitOfWork.SaveChanges() > 0)
                {
                    transaction.Commit();
                    return await GetCar(car.Id) ?? throw new InvalidOperationException("Failed to retrieve car.");
                }

                transaction.Rollback();
                return null!;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
         
        public async Task<CarViewModel> UpdateCar(Guid id, CarUpdateModel model)
        {
            var car = await _carRepository.GetMany(c => c.Id.Equals(id))
                .Include(c => c.Location)
                .Include(c => c.AdditionalCharge)
                .Include(c => c.Model).ThenInclude(m => m.ProductionCompany)
                .FirstOrDefaultAsync();

            if (car == null) return null!;

            car.Name = model.Name ?? car.Name;
            car.Description = model.Description ?? car.Description;
            car.Status = model.Status ?? car.Status;
            car.LicensePlate = model.LicensePlate ?? car.LicensePlate;
            car.Price = model.Price ?? car.Price;

            if (model.Seater != null) car.Model.Seater = (int)model.Seater;
            if (model.FuelType != null) car.Model.FuelType = model.FuelType;
            if (model.FuelConsumption != null) car.Model.FuelConsumption = model.FuelConsumption;
            if (model.TransmissionType != null) car.Model.TransmissionType = model.TransmissionType;
            if (model.YearOfManufacture != null) car.Model.YearOfManufacture = (int)model.YearOfManufacture;
            if (model.ProductionCompanyId != null) car.Model.ProductionCompanyId = (Guid)model.ProductionCompanyId;

            if (model.Location != null)
            {
                car.Location!.Longitude = model.Location.Longitude;
                car.Location!.Latitude = model.Location.Latitude;
            }

            if (model.AdditionalCharge != null)
            {
                car.AdditionalCharge!.TimeSurcharge = model.AdditionalCharge.TimeSurcharge;
                car.AdditionalCharge!.MaximumDistance = model.AdditionalCharge.MaximumDistance;
                car.AdditionalCharge!.DistanceSurcharge = model.AdditionalCharge.DistanceSurcharge;
            }

            _carRepository.Update(car);
            await _unitOfWork.SaveChanges();
            return await GetCar(id);
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

        private async Task<Guid> CreateAdditionalCharge(AdditionalChargeCreateModel model)
        {
            var id = Guid.NewGuid();
            var AdditionalCharge = new AdditionalCharge
            {
                Id = id,
                DistanceSurcharge = model.DistanceSurcharge,
                MaximumDistance = model.MaximumDistance,
                TimeSurcharge = model.TimeSurcharge,
                AdditionalDistance = model.AdditionalDistance,
                AdditionalTime = model.AdditionalTime,
            };
            _additionalChargeRepository.Add(AdditionalCharge);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? id : Guid.Empty;
        }
    }
}
