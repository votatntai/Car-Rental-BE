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
    public class FeedBackService : BaseService, IFeedBackService
    {
        private new readonly IMapper _mapper;
        private readonly IFeedBackRepository _feedBackRepository;
        public FeedBackService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _mapper = mapper;
            _feedBackRepository = unitOfWork.FeedBack;
        }

        public async Task<ListViewModel<FeedBackViewModel>> GetFeedBacks(FeedBackFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _feedBackRepository.GetMany(feedBack => feedBack.OrderId.Equals(filter.OrderId))
            .ProjectTo<FeedBackViewModel>(_mapper.ConfigurationProvider);
            var productionCompanies = await query.Skip(pagination.PageNumber * pagination.PageSize).Take(pagination.PageSize).AsNoTracking().ToListAsync();
            var totalRow = await query.AsNoTracking().CountAsync();
            return productionCompanies != null || productionCompanies != null && productionCompanies.Any() ? new ListViewModel<FeedBackViewModel>
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

        public async Task<FeedBackViewModel> GetFeedBack(Guid id)
        {
            return await _feedBackRepository.GetMany(feedBack => feedBack.Id.Equals(id))
                .ProjectTo<FeedBackViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? null!;
        }

        public async Task<FeedBackViewModel> CreateFeedBack(Guid customerId, FeedBackCreateModel model)
        {
            var feedBack = new FeedBack
            {
                Id = Guid.NewGuid(),
                CarId = model.CarId,
                DriverId = model.DriverId,
                Content = model.Content,
                Star = model.Star,
                CreateAt = DateTime.UtcNow.AddHours(7),
                CustomerId = customerId,
                OrderId = model.OrderId,
            };
            _feedBackRepository.Add(feedBack);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetFeedBack(feedBack.Id) : null!;
        }

        public async Task<FeedBackViewModel> UpdateFeedBack(Guid id, FeedBackUpdateModel model)
        {
            var feedBack = await _feedBackRepository.GetMany(feedBack => feedBack.Id.Equals(id)).FirstOrDefaultAsync();
            if (feedBack == null)
            {
                return null!;
            }
            feedBack.Star = model.Star ?? feedBack.Star;
            feedBack.Content = model.Content ?? feedBack.Content;
            _feedBackRepository.Update(feedBack);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetFeedBack(id) : null!;
        }

        public async Task<bool> DeleteFeedBack(Guid id)
        {
            var feedBack = await _feedBackRepository.GetMany(feedBack => feedBack.Id.Equals(id)).FirstOrDefaultAsync();
            if (feedBack == null)
            {
                return false;
            }
            _feedBackRepository.Remove(feedBack);
            var result = await _unitOfWork.SaveChanges();
            return result > 0;
        }
    }
}
