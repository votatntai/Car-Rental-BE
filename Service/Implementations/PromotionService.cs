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
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Service.Implementations
{
    public class PromotionService : BaseService, IPromotionService
    {
        private new readonly IMapper _mapper;
        private readonly IPromotionRepository _promotionRepository;
        public PromotionService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _mapper = mapper;
            _promotionRepository = unitOfWork.Promotion;
        }

        public async Task<ListViewModel<PromotionViewModel>> GetPromotions(PromotionFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _promotionRepository.GetMany(pc => filter.Name == null || pc.Name != null && pc.Name.Contains(filter.Name));
            if (filter.IsAvailable != null && filter.IsAvailable == true)
            {
                query = query.AsQueryable().Where(p => p.Quantity > 0 && p.ExpiryAt >= DateTime.UtcNow.AddHours(7));
            }
            var productionCompanies = await query.ProjectTo<PromotionViewModel>(_mapper.ConfigurationProvider)
                .Skip(pagination.PageNumber * pagination.PageSize).Take(pagination.PageSize).AsNoTracking().ToListAsync();
            var totalRow = await query.AsNoTracking().CountAsync();
            return productionCompanies != null && totalRow > 0 ? new ListViewModel<PromotionViewModel>
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

        public async Task<PromotionViewModel> GetPromotion(Guid id)
        {
            return await _promotionRepository.GetMany(pc => pc.Id.Equals(id))
                .ProjectTo<PromotionViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? null!;
        }

        public async Task<PromotionViewModel> CreatePromotion(PromotionCreateModel model)
        {
            var promotion = new Promotion
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                Discount = model.Discount,
                Quantity = model.Quantity,
                ExpiryAt = model.ExpiryAt,
            };
            _promotionRepository.Add(promotion);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetPromotion(promotion.Id) : null!;
        }

        public async Task<PromotionViewModel> UpdatePromotion(Guid id, PromotionUpdateModel model)
        {
            var promotion = await _promotionRepository.GetMany(pc => pc.Id.Equals(id)).FirstOrDefaultAsync();
            if (promotion == null)
            {
                return null!;
            }
            promotion.Name = model.Name ?? promotion.Name;
            promotion.Description = model.Description ?? promotion.Description;
            promotion.ExpiryAt = model.ExpiryAt ?? promotion.ExpiryAt;
            promotion.Discount = model.Discount ?? promotion.Discount;
            promotion.Quantity = model.Quantity ?? promotion.Quantity;
            _promotionRepository.Update(promotion);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetPromotion(id) : null!;
        }

        public async Task<bool> DeletePromotion(Guid id)
        {
            var promotion = await _promotionRepository.GetMany(pc => pc.Id.Equals(id)).FirstOrDefaultAsync();
            if (promotion == null)
            {
                return false;
            }
            _promotionRepository.Remove(promotion);
            var result = await _unitOfWork.SaveChanges();
            return result > 0;
        }
    }
}
