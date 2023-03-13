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
        private readonly IAdditionalChargeRepository _AdditionalChargeRepository;
        private new readonly IMapper _mapper;

        public CarService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _carRepository = unitOfWork.Car;
            _locationRepository = unitOfWork.Location;
            _AdditionalChargeRepository = unitOfWork.AdditionalCharge;
            _mapper = mapper;
        }

        public async Task<ListViewModel<CarViewModel>> GetCars(CarFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _carRepository.GetMany(car => car.Name != null && filter.Name != null ? car.Name.Contains(filter.Name) : true);

            if (filter.Location != null)
            {
                query = query.AsQueryable().DistanceFilter(filter.Location.Latitude, filter.Location.Longitude);
            }
            var cars = await query
                .Include(car => car.Model).ThenInclude(model => model.ProductionCompany)
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
            var result = 0;
            var id = Guid.NewGuid();
            using (var transaction = _unitOfWork.Transaction())
            {
                try
                {
                    var locationId = await CreateLocation(model.Location);
                    var AdditionalChargeId = await CreateAdditionalCharge(model.AdditionalCharge);

                    var car = new Car
                    {
                        Id = id,
                        Description = model.Description,
                        LocationId = locationId,
                        AdditionalChargeId = AdditionalChargeId,
                        LicensePlate = model.LicensePlate,
                        Name = model.Name,
                        Price = model.Price,
                        Status = CarStatus.Idle.ToString(),
                        ModelId = model.ModelId,
                        ReceiveTime = model.ReceiveTime,
                        ReturnTime = model.ReturnTime,
                        Rented = 0,
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
                .Include(car => car.AdditionalCharge)
                .Include(car => car.Model).ThenInclude(model => model.ProductionCompany)
                .FirstOrDefaultAsync();
            if (car != null)
            {
                if (model.Name != null) car.Name = model.Name;
                if (model.Seater != null) car.Model.Seater = (int)model.Seater;
                if (model.FuelType != null) car.Model.FuelType = model.FuelType;
                if (model.FuelConsumption != null) car.Model.FuelConsumption = model.FuelConsumption;
                if (model.Description != null) car.Description = model.Description;
                if (model.Status != null) car.Status = model.Status;
                if (model.LicensePlate != null) car.LicensePlate = model.LicensePlate;
                if (model.TransmissionType != null) car.Model.TransmissionType = model.TransmissionType;
                if (model.YearOfManufacture != null) car.Model.YearOfManufacture = (int)model.YearOfManufacture;
                if (model.ProductionCompanyId != null) car.Model.ProductionCompanyId = (Guid)model.ProductionCompanyId;
                if (model.Price != null) car.Price = (double)model.Price;
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

        private async Task<Guid> CreateAdditionalCharge(AdditionalChargeCreateModel model)
        {
            var id = Guid.NewGuid();
            var AdditionalCharge = new AdditionalCharge
            {
                Id = id,
                DistanceSurcharge = model.DistanceSurcharge,
                MaximumDistance = model.MaximumDistance,
                TimeSurcharge = model.TimeSurcharge
            };
            _AdditionalChargeRepository.Add(AdditionalCharge);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? id : Guid.Empty;
        }
    }
}
