using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Data.Entities;
using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using Utility.Enums;

namespace Service.Implementations
{
    public class CarRegistrationService : BaseService, ICarRegistrationService
    {
        private readonly ICarRegistrationRepository _carRegistrationRepository;
        private readonly IAdditionalChargeRepository _additionalChargeRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly INotificationService _notificationService;
        private new readonly IMapper _mapper;
        public CarRegistrationService(IUnitOfWork unitOfWork, IMapper mapper,
            ICloudStorageService cloudStorageService, INotificationService notificationService)
            : base(unitOfWork, mapper)
        {
            _carRegistrationRepository = unitOfWork.CarRegistration;
            _additionalChargeRepository = unitOfWork.AdditionalCharge;
            _imageRepository = unitOfWork.Image;
            _userRepository = unitOfWork.User;  
            _mapper = mapper;
            _cloudStorageService = cloudStorageService;
            _notificationService = notificationService;
        }

        public async Task<ListViewModel<CarRegistrationViewModel>> GetCarRegistrations(CarRegistrationFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _carRegistrationRepository.GetMany(carRegistration => filter.Name != null && carRegistration.Name != null ?
                carRegistration.Name.Contains(filter.Name) : true)
                .ProjectTo<CarRegistrationViewModel>(_mapper.ConfigurationProvider);
            var carRegistrations = await query.OrderByDescending(carRegistration => carRegistration.CreateAt)
                .Skip(pagination.PageNumber * pagination.PageSize).Take(pagination.PageSize).AsNoTracking().ToListAsync();
            var totalRow = await query.AsNoTracking().CountAsync();
            if (carRegistrations != null || carRegistrations != null && carRegistrations.Any())
            {
                return new ListViewModel<CarRegistrationViewModel>
                {
                    Pagination = new PaginationViewModel
                    {
                        TotalRow = totalRow,
                        PageNumber = pagination.PageNumber,
                        PageSize = pagination.PageSize,
                    },
                    Data = carRegistrations
                };
            }
            return null!;
        }

        public async Task<CarRegistrationViewModel> GetCarRegistration(Guid id)
        {
            return await _carRegistrationRepository.GetMany(carRegistration => carRegistration.Id.Equals(id))
                .Include(carRegistration => carRegistration.CarRegistrationCalendars)
                .ThenInclude(carRegistrationCalendar => carRegistrationCalendar.Calendar)
                .ProjectTo<CarRegistrationViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? null!;
        }

        public async Task<CarRegistrationViewModel> CreateCarRegistration
            (Guid carOwnerId, ICollection<IFormFile> images, ICollection<IFormFile> licenses, CarRegistrationCreateModel model)
        {
            using var transaction = _unitOfWork.Transaction();
            try
            {
                var additionalChargeId = await CreateAdditionalCharge(model.AdditionalCharge);
                var carRegistration = new CarRegistration
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Description = model.Description,
                    FuelConsumption = model.FuelConsumption,
                    FuelType = model.FuelType,
                    LicensePlate = model.LicensePlate,
                    Location = model.Location,
                    Price = model.Price,
                    ProductionCompany = model.ProductionCompany,
                    Seater = model.Seater,
                    Chassis = model.Chassis,
                    TransmissionType = model.TransmissionType,
                    YearOfManufacture = model.YearOfManufacture,
                    Model = model.Model,
                    CreateAt = DateTime.UtcNow.AddHours(7),
                    AdditionalChargeId = additionalChargeId,
                    CarOwnerId = carOwnerId,
                    Status = false,
                };
                _carRegistrationRepository.Add(carRegistration);
                if (await _unitOfWork.SaveChanges() > 0)
                {
                    await CreateCarRegistrationLicenses(carRegistration.Id, licenses);
                    await CreateCarRegistrationImages(carRegistration.Id, images);
                    var message = new NotificationCreateModel
                    {
                        Title = "Có đơn đăng ký mới cần xác nhận",
                        Body = "Có một phiếu đăng ký xe mới đang chờ xác nhận",
                        Data = new NotificationDataViewModel
                        {
                            CreateAt = DateTime.UtcNow.AddHours(7),
                            Type = NotificationType.CarRegistration.ToString(),
                            IsRead = false,
                            Link = carRegistration.Id.ToString(),
                        }
                    };
                    var admins = await _userRepository.GetMany(user => user.Role.Equals(UserRole.Manager.ToString())).Select(admin => admin.AccountId).ToListAsync();
                    await _notificationService.SendNotification(admins, message);
                    transaction.Commit();
                    return await GetCarRegistration(carRegistration.Id) ?? throw new InvalidOperationException("Failed to retrieve car registration.");
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
                    CarRegistrationId = id,
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
                    CarRegistrationId = id,
                    Type = ImageType.License.ToString(),
                    Url = url,
                };
                images.Add(image);
            }
            _imageRepository.AddRange(images);
            return await _unitOfWork.SaveChanges() > 0 ? images : null!;
        }

        public async Task<CarRegistrationViewModel> UpdateCar(Guid id, CarRegistrationUpdateModel model)
        {
            var carRegistration = await _carRegistrationRepository.GetMany(c => c.Id.Equals(id))
                .FirstOrDefaultAsync();
            if (carRegistration == null) return null!;
            carRegistration.Status = model.IsApproved ?? carRegistration.Status;
            carRegistration.Description = model.Description ?? carRegistration.Description;
            _carRegistrationRepository.Update(carRegistration);
            if (await _unitOfWork.SaveChanges() > 0)
            {
                var acceptMessage = new NotificationCreateModel
                {
                    Title = "Phiếu đăng ký xe",
                    Body = "Phiếu đăng ký của bạn đã được phê duyệt",
                    Data = new NotificationDataViewModel
                    {
                        CreateAt = DateTime.UtcNow.AddHours(7),
                        Type = NotificationType.CarRegistration.ToString(),
                        IsRead = false,
                        Link = carRegistration.Id.ToString(),
                    }
                };
                var denyMessage = new NotificationCreateModel
                {
                    Title = "Phiếu đăng ký xe",
                    Body = "Phiếu đăng ký của bạn đã bị từ chối",
                    Data = new NotificationDataViewModel
                    {
                        CreateAt = DateTime.UtcNow.AddHours(7),
                        Type = NotificationType.CarRegistration.ToString(),
                        IsRead = false,
                        Link = carRegistration.Id.ToString(),
                    }
                };
                var carOwnerIds = new List<Guid> { carRegistration.CarOwnerId };
                await _notificationService.SendNotification(carOwnerIds, carRegistration.Status ? acceptMessage : denyMessage);
            }
            return await GetCarRegistration(id);
        }

        public async Task<bool> DeleteCarRegistration(Guid id)
        {
            var result = false;
            if (_carRegistrationRepository.Any(carRegistration => carRegistration.Id.Equals(id)))
            {
                var carRegistration = await _carRegistrationRepository.GetMany(carRegistration => carRegistration.Id.Equals(id)).FirstOrDefaultAsync();
                _carRegistrationRepository.Remove(carRegistration!);
                result = await _unitOfWork.SaveChanges() > 0;
            }
            return result;
        }

        // PRIVATE METHOD
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
