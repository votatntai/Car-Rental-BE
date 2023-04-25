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
    public class ModelService : BaseService, IModelService
    {
        private readonly IModelRepository _modelRepository;
        private new readonly IMapper _mapper;
        public ModelService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _modelRepository = unitOfWork.Model;
            _mapper = mapper;
        }

        public async Task<ListViewModel<CarModelViewModel>> GetModels(ModelFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _modelRepository.GetMany(pc => filter.Name != null ? pc.Name.Contains(filter.Name) ||
                pc.ProductionCompany.Name.Contains(filter.Name) : true &&
                filter.ProductionCompanyId != null ? pc.ProductionCompanyId.Equals(filter.ProductionCompanyId) : true)
            .ProjectTo<CarModelViewModel>(_mapper.ConfigurationProvider);
            var models = await query
                .Skip(pagination.PageNumber * pagination.PageSize).Take(pagination.PageSize).AsNoTracking().ToListAsync();
            var totalRow = await query.AsNoTracking().CountAsync();
            return models != null && totalRow > 0 ? new ListViewModel<CarModelViewModel>
            {
                Pagination = new PaginationViewModel
                {
                    TotalRow = totalRow,
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                },
                Data = models
            } : null!;
        }

        public async Task<CarModelViewModel> GetModel(Guid id)
        {
            return await _modelRepository.GetMany(pc => pc.Id.Equals(id))
                .ProjectTo<CarModelViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? null!;
        }

        public async Task<CarModelViewModel> CreateModel(ModelCreateModel model)
        {
            var carModel = new Model
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                CellingPrice = model.CellingPrice,
                FloorPrice = model.FloorPrice,
                FuelConsumption = model.FuelConsumption,
                FuelType = model.FuelType,
                Chassis = model.Chassis,
                ProductionCompanyId = model.ProductionCompanyId,
                Seater = model.Seater,
                TransmissionType = model.TransmissionType,
                YearOfManufacture = model.YearOfManufacture,
            };
            _modelRepository.Add(carModel);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetModel(carModel.Id) : null!;
        }

        public async Task<CarModelViewModel> UpdateModel(Guid id, ModelUpdateModel model)
        {
            var carModel = await _modelRepository.GetMany(pc => pc.Id.Equals(id)).FirstOrDefaultAsync();
            if (carModel == null)
            {
                return null!;
            }
            carModel.Name = model.Name ?? carModel.Name;
            carModel.Seater = model.Seater ?? carModel.Seater;
            carModel.FloorPrice = model.FloorPrice ?? carModel.FloorPrice;
            carModel.YearOfManufacture = model.YearOfManufacture ?? carModel.YearOfManufacture;
            carModel.ProductionCompanyId = model.ProductionCompanyId ?? carModel.ProductionCompanyId;
            carModel.Chassis = model.Chassis ?? carModel.Chassis;
            carModel.CellingPrice = model.CellingPrice ?? carModel.CellingPrice;
            carModel.FuelConsumption = model.FuelConsumption ?? carModel.FuelConsumption;
            carModel.FuelType = model.FuelType ?? carModel.FuelType;
            carModel.TransmissionType = model.TransmissionType ?? carModel.TransmissionType;

            _modelRepository.Update(carModel);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetModel(id) : null!;
        }

        public async Task<bool> DeleteModel(Guid id)
        {
            var model = await _modelRepository.GetMany(pc => pc.Id.Equals(id)).FirstOrDefaultAsync();
            if (model == null)
            {
                return false;
            }
            _modelRepository.Remove(model);
            var result = await _unitOfWork.SaveChanges();
            return result > 0;
        }

    }
}
