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
    public class ProductionCompanyService : BaseService, IProductionCompanyService
    {
        private readonly IProductionCompanyRepository _productionCompanyRepository;
        private new readonly IMapper _mapper;
        public ProductionCompanyService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _productionCompanyRepository = unitOfWork.ProductionCompany;
            _mapper = mapper;
        }

        public async Task<ListViewModel<ProductionCompanyViewModel>> GetProductionCompanies(ProductionCompanyFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _productionCompanyRepository.GetMany(pc => filter.Name == null || pc.Name != null && pc.Name.Contains(filter.Name))
            .ProjectTo<ProductionCompanyViewModel>(_mapper.ConfigurationProvider);
            var productionCompanies = await query.Skip(pagination.PageNumber * pagination.PageSize).Take(pagination.PageSize).AsNoTracking().ToListAsync();
            var totalRow = await query.AsNoTracking().CountAsync();
            return productionCompanies != null && totalRow > 0 ? new ListViewModel<ProductionCompanyViewModel>
            {
                Pagination = new PaginationViewModel
                {
                    TotalRow = totalRow,
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                },
                Data = productionCompanies
            } : null!;
        }

        public async Task<ProductionCompanyViewModel> GetProductionCompany(Guid id)
        {
            return await _productionCompanyRepository.GetMany(pc => pc.Id.Equals(id))
                .ProjectTo<ProductionCompanyViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? null!;
        }

        public async Task<ProductionCompanyViewModel> CreateProductionCompany(ProductionCompanyCreateModel model)
        {
            var productionCompany = new ProductionCompany
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
            };
            _productionCompanyRepository.Add(productionCompany);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetProductionCompany(productionCompany.Id) : null!;
        }

        public async Task<ProductionCompanyViewModel> UpdateProductionCompany(Guid id, ProductionCompanyUpdateModel model)
        {
            var productionCompany = await _productionCompanyRepository.GetMany(pc => pc.Id.Equals(id)).FirstOrDefaultAsync();
            if (productionCompany == null)
            {
                return null!;
            }
            productionCompany.Name = model.Name ?? productionCompany.Name;
            productionCompany.Description = model.Description ?? productionCompany.Description;
            _productionCompanyRepository.Update(productionCompany);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetProductionCompany(id) : null!;
        }

        public async Task<bool> DeleteProductionCompany(Guid id)
        {
            var productionCompany = await _productionCompanyRepository.GetMany(pc => pc.Id.Equals(id)).FirstOrDefaultAsync();
            if (productionCompany == null)
            {
                return false;
            }
            _productionCompanyRepository.Remove(productionCompany);
            var result = await _unitOfWork.SaveChanges();
            return result > 0;
        }

    }
}
