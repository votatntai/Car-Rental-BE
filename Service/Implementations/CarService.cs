using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Data.Entities;
using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;
using Data.Repositories.Interfaces;
using Extensions.MyExtentions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using Utility.Enums;

namespace Service.Implementations
{
    public class CarService : BaseService, ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ICalendarRepository _calendarRepository;
        private readonly ICarCalendarRepository _carCalendarRepository;
        private readonly IAdditionalChargeRepository _additionalChargeRepository;
        private readonly IImageRepository _imageRepository;
        private readonly ICloudStorageService _cloudStorageService;
        private new readonly IMapper _mapper;

        public CarService(IUnitOfWork unitOfWork, IMapper mapper, ICloudStorageService cloudStorageService) : base(unitOfWork, mapper)
        {
            _carRepository = unitOfWork.Car;
            _locationRepository = unitOfWork.Location;
            _calendarRepository = unitOfWork.Calendar;
            _carCalendarRepository = unitOfWork.CarCalendar;
            _additionalChargeRepository = unitOfWork.AdditionalCharge;
            _imageRepository = unitOfWork.Image;
            _mapper = mapper;
            _cloudStorageService = cloudStorageService;
        }

        public async Task<ListViewModel<CarViewModel>> GetCars(CarFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _carRepository.GetMany(car => car.Name != null && filter.Name != null ? (car.Name.Contains(filter.Name) ||
            car.Model.ProductionCompany.Name.Contains(filter.Name)) : true);
            if (filter.IsAvailable != null && filter.IsAvailable == true)
            {
                query = query.AsQueryable().Where(car => !car.Status.Equals(CarStatus.InOrder.ToString()) && !car.Status.Equals(CarStatus.Blocked.ToString()));
            }
            if (filter.HasShowroom != null && filter.HasShowroom == true)
            {
                query = query.AsQueryable().Where(car => car.Showroom != null);
            }
            if (filter.HasShowroom != null && filter.HasShowroom == false)
            {
                query = query.AsQueryable().Where(car => car.Showroom == null);
            }
            if (filter.Status != null)
            {
                query = query.AsQueryable().Where(car => car.Status.Equals(filter.Status.ToString()));
            }
            if (filter.Location != null)
            {
                query = query.AsQueryable().DistanceFilter(filter.Location.Latitude, filter.Location.Longitude, filter.Distance != null ? filter.Distance : 5);
            }
            if (filter.CarType != null)
            {
                query = query.AsQueryable().Where(car => car.CarTypes.Any(carType => carType.Type.Name.Equals(filter.CarType.ToString())));
            }
            if (filter.ModelId != null)
            {
                query = query.AsQueryable().Where(car => car.ModelId.Equals(filter.ModelId));
            }
            if (filter.ProductionCompanyId != null)
            {
                query = query.AsQueryable().Where(car => car.Model.ProductionCompanyId.Equals(filter.ProductionCompanyId));
            }
            if (filter.Price != null)
            {
                query = query.AsQueryable().Where(car => car.Price >= filter.Price.MinPrice && car.Price <= filter.Price.MaxPrice);
            }
            if (filter.TransmissionType != null)
            {
                query = query.AsQueryable().Where(car => car.Model.TransmissionType.Equals(filter.TransmissionType.ToString()));
            }
            if (filter.StartTime != null && filter.EndTime != null)
            {
                var startTime = new TimeSpan(filter.StartTime.Value.Hour, filter.StartTime.Value.Minute, filter.StartTime.Value.Second);
                var endTime = new TimeSpan(filter.EndTime.Value.Hour, filter.StartTime.Value.Minute, filter.StartTime.Value.Second);
                query = query.AsQueryable().Where(car => car.ReceiveStartTime <= startTime && car.ReceiveEndTime >= endTime &&
                car.OrderDetails.Any(od => od.CarId.Equals(car.Id)) ?
                car.OrderDetails.Any(od => (od.StartTime > filter.EndTime.Value.AddDays(-1) || od.EndTime < filter.StartTime.Value.AddDays(-1))) : true);
            }
            var cars = await query
            .OrderByDescending(car => car.CreateAt)
            .ThenByDescending(car => car.Star)
            .ThenByDescending(car => car.Rented)
            .ProjectTo<CarViewModel>(_mapper.ConfigurationProvider)
            .Skip(pagination.PageNumber * pagination.PageSize).Take(pagination.PageSize)
            .AsNoTracking()
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
                .ProjectTo<CarViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? null!;
        }

        public async Task<ICollection<CarCalendarViewModel>> GetCarCalendar(Guid id)
        {
            return await _carCalendarRepository.GetMany(cld => cld.CarId.Equals(id))
                .ProjectTo<CarCalendarViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync() ?? null!;
        }

        public async Task<CarViewModel> CreateCar(CarCreateModel model)
        {
            if (_carRepository.Any(car => car.LicensePlate.Equals(model.LicensePlate)))
            {
                return null!;
            }
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
                    CarOwnerId = model.CarOwnerId,
                    Rented = 0,
                    CreateAt = DateTime.UtcNow.AddHours(7)
                };

                foreach (var weekday in Enum.GetValues(typeof(Weekday)))
                {
                    var calendar = new Calendar
                    {
                        Id = Guid.NewGuid(),
                        StartTime = TimeSpan.Parse("08:00:00"),
                        EndTime = TimeSpan.Parse("20:00:00"),
                        Weekday = weekday.ToString() ?? null!,
                    };

                    if (model.Calendars != null)
                    {
                        var item = model.Calendars.FirstOrDefault(c => c.Calendar.Weekday.Equals(weekday.ToString()));
                        if (item != null)
                        {
                            calendar.StartTime = item.Calendar.StartTime;
                            calendar.EndTime = item.Calendar.EndTime;
                        }
                    }

                    _calendarRepository.Add(calendar);
                    var carCalendar = new CarCalendar
                    {
                        CalendarId = calendar.Id,
                        CarId = car.Id,
                    };
                    _carCalendarRepository.Add(carCalendar);
                }

                _carRepository.Add(car);

                if (await _unitOfWork.SaveChanges() > 0)
                {
                    if (model.RegistrationId != null)
                    {
                        var images = await _imageRepository.GetMany(image => image.CarRegistrationId.Equals(model.RegistrationId)).ToListAsync();
                        if (images != null)
                        {
                            foreach (var image in images)
                            {
                                image.CarId = car.Id;
                            }
                            _imageRepository.UpdateRange(images);
                            await _unitOfWork.SaveChanges();
                        }
                    }
                    transaction.Commit();
                    return await GetCar(car.Id) ?? throw new InvalidOperationException("Failed to retrieve car.");
                }
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

            transaction.Rollback();
            return null!;
        }

        public async Task<CarViewModel> CreateShowroomCar(ICollection<IFormFile> images, ICollection<IFormFile> licenses, CarShowroomCreateModel model)
        {
            if (_carRepository.Any(car => car.LicensePlate.Equals(model.LicensePlate)))
            {
                return null!;
            }
            using var transaction = _unitOfWork.Transaction();
            try
            {
                var location = await _locationRepository.GetMany(location => location.Showrooms.Select(showroom => showroom.Id.Equals(model.ShowroomId)).FirstOrDefault()).FirstOrDefaultAsync();
                var additionalChargeId = await CreateAdditionalCharge(null);
                var car = new Car
                {
                    Id = Guid.NewGuid(),
                    Description = model.Description,
                    LocationId = location!.Id,
                    LicensePlate = model.LicensePlate,
                    AdditionalChargeId = additionalChargeId,
                    Name = model.Name,
                    Price = model.Price,
                    CarOwnerId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                    Status = CarStatus.Idle.ToString(),
                    ModelId = model.ModelId,
                    Rented = 0,
                    ShowroomId = model.ShowroomId,
                    CreateAt = DateTime.UtcNow.AddHours(7)
                };

                foreach (var weekday in Enum.GetValues(typeof(Weekday)))
                {
                    var calendar = new Calendar
                    {
                        Id = Guid.NewGuid(),
                        StartTime = TimeSpan.Parse("08:00:00"),
                        EndTime = TimeSpan.Parse("20:00:00"),
                        Weekday = weekday.ToString() ?? null!,
                    };

                    _calendarRepository.Add(calendar);
                    var carCalendar = new CarCalendar
                    {
                        CalendarId = calendar.Id,
                        CarId = car.Id,
                    };
                    _carCalendarRepository.Add(carCalendar);
                }

                _carRepository.Add(car);

                if (await _unitOfWork.SaveChanges() > 0)
                {
                    await CreateCarRegistrationLicenses(car.Id, licenses);
                    await CreateCarRegistrationImages(car.Id, images);
                    transaction.Commit();
                    return await GetCar(car.Id) ?? throw new InvalidOperationException("Failed to retrieve car.");
                }
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

            transaction.Rollback();
            return null!;
        }

        public async Task<CarViewModel> UpdateCar(Guid id, CarUpdateModel model)
        {
            var car = await _carRepository.GetMany(c => c.Id.Equals(id))
                .Include(c => c.Location)
                .Include(c => c.AdditionalCharge)
                .Include(c => c.Model).ThenInclude(m => m.ProductionCompany)
                .Include(c => c.CarCalendars).ThenInclude(cc => cc.Calendar)
                .FirstOrDefaultAsync();

            if (car == null) return null!;

            car.Name = model.Name ?? car.Name;
            car.Description = model.Description ?? car.Description;
            car.Status = model.Status.ToString() ?? car.Status;
            car.LicensePlate = model.LicensePlate ?? car.LicensePlate;
            car.Price = model.Price ?? car.Price;
            car.DriverId = model.DriverId ?? car.DriverId;

            if (model.Seater != null) car.Model.Seater = (int)model.Seater;
            if (model.FuelType != null) car.Model.FuelType = model.FuelType;
            if (model.FuelConsumption != null) car.Model.FuelConsumption = model.FuelConsumption;
            if (model.TransmissionType != null) car.Model.TransmissionType = model.TransmissionType;
            if (model.YearOfManufacture != null) car.Model.YearOfManufacture = (int)model.YearOfManufacture;
            if (model.ProductionCompanyId != null) car.Model.ProductionCompanyId = (Guid)model.ProductionCompanyId;

            if (model.Location != null)
            {
                car.Location!.Latitude = model.Location.Latitude;
                car.Location!.Longitude = model.Location.Longitude;
            }

            if (model.AdditionalCharge != null)
            {
                car.AdditionalCharge!.TimeSurcharge = model.AdditionalCharge.TimeSurcharge;
                car.AdditionalCharge!.MaximumDistance = model.AdditionalCharge.MaximumDistance;
                car.AdditionalCharge!.DistanceSurcharge = model.AdditionalCharge.DistanceSurcharge;
            }

            if (model.CarCalendars != null)
            {
                foreach (var item in car.CarCalendars)
                {
                    var updateItem = model.CarCalendars.FirstOrDefault(x => x.Calendar.Id == item.Calendar.Id);
                    if (updateItem != null && updateItem.Calendar.Id.Equals(item.Calendar.Id))
                    {
                        var calendar = await _calendarRepository.GetMany(calendar => calendar.Id.Equals(updateItem.Calendar.Id)).FirstOrDefaultAsync();
                        if (calendar != null)
                        {
                            calendar.StartTime = updateItem.Calendar.StartTime ?? calendar.StartTime;
                            calendar.EndTime = updateItem.Calendar.EndTime ?? calendar.EndTime;
                            _calendarRepository.Update(calendar);
                        }
                    }
                }
            }

            _carRepository.Update(car);
            await _unitOfWork.SaveChanges();
            return await GetCar(id);
        }

        public async Task<ListViewModel<CarViewModel>> GetCarsByCarOwnerId(Guid carOwnerId, CarFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _carRepository.GetMany(car => car.CarOwnerId.Equals(carOwnerId));
            if (filter.Status != null)
            {
                query = query.Where(car => car.Status.Equals(filter.Status.ToString()));
            };
            var totalRow = await query.AsNoTracking().CountAsync();
            var cars = await query.ProjectTo<CarViewModel>(_mapper.ConfigurationProvider)
                .Skip(pagination.PageNumber * pagination.PageSize).Take(pagination.PageSize)
                .AsNoTracking().ToListAsync();
            return new ListViewModel<CarViewModel>
            {
                Data = cars,
                Pagination = new PaginationViewModel
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalRow = totalRow,
                },
            };
        }

        public async Task<ICollection<CarViewModel>> GetCarsIsNotTracking(Guid carOwnerId, PaginationRequestModel pagination)
        {
            return await _carRepository.GetMany(car => car.CarOwnerId.Equals(carOwnerId) && !car.IsTracking)
                .ProjectTo<CarViewModel>(_mapper.ConfigurationProvider)
                .Skip(pagination.PageNumber * pagination.PageSize).Take(pagination.PageSize)
                .ToListAsync();
        }

        public async Task<CarViewModel> TrackingACar(Guid carId)
        {
            var car = await _carRepository.GetMany(car => car.Id.Equals(carId)).FirstOrDefaultAsync();
            if (car != null)
            {
                car.IsTracking = true;
                _carRepository.Update(car);
                return await _unitOfWork.SaveChanges() > 0 ? await GetCar(carId) : null!;
            }
            return null!;
        }

        public async Task<CarViewModel> CancelTrackingACar(Guid carId)
        {
            var car = await _carRepository.GetMany(car => car.Id.Equals(carId)).FirstOrDefaultAsync();
            if (car != null)
            {
                car.IsTracking = false;
                _carRepository.Update(car);
                return await _unitOfWork.SaveChanges() > 0 ? await GetCar(carId) : null!;
            }
            return null!;
        }

        // PRIVATE METHODS

        private async Task<ICollection<Image>> CreateCarRegistrationImages(Guid id, ICollection<IFormFile> files)
        {
            var images = new List<Image>();
            foreach (IFormFile file in files)
            {
                var imageId = Guid.NewGuid();
                var url = await _cloudStorageService.Upload(imageId, file.ContentType, file.OpenReadStream());
                var image = new Image
                {
                    Id = imageId,
                    CarId = id,
                    Type = ImageType.Thumbnail.ToString(),
                    Url = url,
                };
                images.Add(image);
            }
            _imageRepository.AddRange(images);
            return await _unitOfWork.SaveChanges() > 0 ? images : null!;
        }

        private async Task<ICollection<Image>> CreateCarRegistrationLicenses(Guid id, ICollection<IFormFile> files)
        {
            var images = new List<Image>();
            foreach (IFormFile file in files)
            {
                var imageId = Guid.NewGuid();
                var url = await _cloudStorageService.Upload(imageId, file.ContentType, file.OpenReadStream());
                var image = new Image
                {
                    Id = imageId,
                    CarId = id,
                    Type = ImageType.License.ToString(),
                    Url = url,
                };
                images.Add(image);
            }
            _imageRepository.AddRange(images);
            return await _unitOfWork.SaveChanges() > 0 ? images : null!;
        }

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

        private async Task<Guid> CreateAdditionalCharge(AdditionalChargeCreateModel? model)
        {
            var id = Guid.NewGuid();
            var additionalCharge = new AdditionalCharge
            {
                Id = id,
                DistanceSurcharge = model != null ? model.DistanceSurcharge : 200000,
                MaximumDistance = model != null ? model.MaximumDistance : 100,
                TimeSurcharge = model != null ? model.TimeSurcharge : 100000,
                AdditionalDistance = model != null ? model.AdditionalDistance : 10,
                AdditionalTime = model != null ? model.AdditionalTime : 12,
            };
            _additionalChargeRepository.Add(additionalCharge);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? id : Guid.Empty;
        }
    }
}
