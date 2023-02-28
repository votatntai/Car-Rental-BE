using Data;
using Data.Entities;
using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Views;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Service.Implementations
{
    public class CarRegistrationService : BaseService, ICarRegistrationService
    {
        private readonly ICarRegistrationRepository _carRegistrationRepository;
        public CarRegistrationService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _carRegistrationRepository = unitOfWork.CarRegistration;
        }

        public async Task<ListViewModel<CarRegistrationViewModel>> GetCarRegistrations(CarRegistrationFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _carRegistrationRepository.GetMany(carRegistration => filter.Name != null && carRegistration.Name != null ?
                carRegistration.Name.Contains(filter.Name) : true).Select(carRegistration => new CarRegistrationViewModel
                {
                    Id = carRegistration.Id,
                    CreateAt = DateTime.Now,
                    Description = carRegistration.Description,
                    FuelConsumption = carRegistration.FuelConsumption,
                    FuelType = carRegistration.FuelType,
                    LicensePlate = carRegistration.LicensePlate,
                    Location = carRegistration.Location,
                    Price = carRegistration.Price,
                    ProductionCompany = carRegistration.ProductionCompany,
                    Seater = carRegistration.Seater,
                    TransmissionType = carRegistration.TransmissionType,
                    Type = carRegistration.Type,
                    Name = carRegistration.Name,
                    YearOfManufacture = carRegistration.YearOfManufacture,
                    ImageUrls = carRegistration.Images.Select(image => image.Url).ToList(),
                });
            var carRegistrations = await query.OrderBy(carRegistration => carRegistration.CreateAt)
                .Skip(pagination.PageNumber * pagination.PageSize).Take(pagination.PageSize).AsNoTracking().ToListAsync();
            var totalRow = await query.AsNoTracking().CountAsync();
            if (carRegistrations != null && totalRow > 0)
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
                .Select(carRegistration => new CarRegistrationViewModel
                {
                    Id = carRegistration.Id,
                    CreateAt = DateTime.Now,
                    Description = carRegistration.Description,
                    FuelConsumption = carRegistration.FuelConsumption,
                    FuelType = carRegistration.FuelType,
                    LicensePlate = carRegistration.LicensePlate,
                    Location = carRegistration.Location,
                    Price = carRegistration.Price,
                    ProductionCompany = carRegistration.ProductionCompany,
                    Seater = carRegistration.Seater,
                    TransmissionType = carRegistration.TransmissionType,
                    Type = carRegistration.Type,
                    Name = carRegistration.Name,
                    YearOfManufacture = carRegistration.YearOfManufacture,
                    ImageUrls = carRegistration.Images.Select(image => image.Url).ToList(),
                }).FirstOrDefaultAsync() ?? null!;
        }

        public async Task<CarRegistrationViewModel> CreateCarRegistration(CarRegistrationCreateModel model)
        {
            var id = Guid.NewGuid();
            var carRegistration = new CarRegistration
            {
                Id = id,
                Name = model.Name,
                Description = model.Description,
                FuelConsumption = model.FuelConsumption,
                FuelType = model.FuelType,
                LicensePlate = model.LicensePlate,
                Location = model.Location,  
                Price = model.Price,    
                ProductionCompany = model.ProductionCompany,
                Seater = model.Seater,
                TransmissionType = model.TransmissionType,
                Type = model.Type,
                YearOfManufacture = model.YearOfManufacture,
                CreateAt = DateTime.Now,
            };
            _carRegistrationRepository.Add(carRegistration);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetCarRegistration(id) : null!;
        }

        public async Task<bool> DeleteCarRegistration(Guid id)
        {
            var result = false;
            if(_carRegistrationRepository.Any(carRegistration => carRegistration.Id.Equals(id)))
            {
                var carRegistration = await _carRegistrationRepository.GetMany(carRegistration => carRegistration.Id.Equals(id)).FirstOrDefaultAsync();
                _carRegistrationRepository.Remove(carRegistration!);
                result = await _unitOfWork.SaveChanges() > 0;
            }
            return result;
        }
    }
}
