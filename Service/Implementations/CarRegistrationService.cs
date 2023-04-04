using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Data.Entities;
using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Service.Implementations
{
    public class CarRegistrationService : BaseService, ICarRegistrationService
    {
        private readonly ICarRegistrationRepository _carRegistrationRepository;
        private readonly IAdditionalChargeRepository _additionalChargeRepository;
        private new readonly IMapper _mapper;
        public CarRegistrationService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _carRegistrationRepository = unitOfWork.CarRegistration;
            _additionalChargeRepository = unitOfWork.AdditionalCharge;
            _mapper = mapper;
        }

        public async Task<ListViewModel<CarRegistrationViewModel>> GetCarRegistrations(CarRegistrationFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _carRegistrationRepository.GetMany(carRegistration => filter.Name != null && carRegistration.Name != null ?
                carRegistration.Name.Contains(filter.Name) : true)
                .ProjectTo<CarRegistrationViewModel>(_mapper.ConfigurationProvider);
            var carRegistrations = await query.OrderBy(carRegistration => carRegistration.CreateAt)
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

        public async Task<CarRegistrationViewModel> CreateCarRegistration(Guid carOwnerId, CarRegistrationCreateModel model)
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
                    CreateAt = DateTime.Now,
                    AdditionalChargeId = additionalChargeId,
                    CarOwnerId = carOwnerId,
                    Status = false,
                };
                _carRegistrationRepository.Add(carRegistration);

                if (await _unitOfWork.SaveChanges() > 0)
                {
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

        public async Task<CarRegistrationViewModel> UpdateCar(Guid id, CarRegistrationUpdateModel model)
        {
            var carRegistration = await _carRegistrationRepository.GetMany(c => c.Id.Equals(id))
                .FirstOrDefaultAsync();
            if (carRegistration == null) return null!;
            carRegistration.Status = model.IsApproved ?? carRegistration.Status;
            _carRegistrationRepository.Update(carRegistration);
            await _unitOfWork.SaveChanges();
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
