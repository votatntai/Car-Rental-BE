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
        private readonly IDriverRepository _driverRepository;
        private readonly ICarRepository _carRepository;
        public FeedBackService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _mapper = mapper;
            _feedBackRepository = unitOfWork.FeedBack;
            _carRepository = unitOfWork.Car;
            _driverRepository = unitOfWork.Driver;
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

        public async Task<ListViewModel<FeedBackViewModel>> GetFeedBacksForDriver(Guid id, PaginationRequestModel pagination)
        {
            var query = _feedBackRepository.GetMany(feedBack => feedBack.DriverId.Equals(id));
            var feedbacks = await query.Skip(pagination.PageNumber * pagination.PageSize).Take(pagination.PageSize).AsNoTracking()
                .ProjectTo<FeedBackViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            var totalRow = await query.AsNoTracking().CountAsync();
            if (feedbacks != null || feedbacks != null && feedbacks.Any())
            {
                return new ListViewModel<FeedBackViewModel>
                {
                    Pagination = new PaginationViewModel
                    {
                        PageNumber = pagination.PageNumber,
                        PageSize = pagination.PageSize,
                        TotalRow = totalRow
                    },
                    Data = feedbacks
                };
            }
            return null!;
        }

        public async Task<ListViewModel<FeedBackViewModel>> GetFeedBacksForCar(Guid id, PaginationRequestModel pagination)
        {
            var query = _feedBackRepository.GetMany(feedBack => feedBack.CarId.Equals(id));
            var feedbacks = await query.Skip(pagination.PageNumber * pagination.PageSize).Take(pagination.PageSize).AsNoTracking()
                .ProjectTo<FeedBackViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            var totalRow = await query.AsNoTracking().CountAsync();
            if (feedbacks != null || feedbacks != null && feedbacks.Any())
            {
                return new ListViewModel<FeedBackViewModel>
                {
                    Pagination = new PaginationViewModel
                    {
                        PageNumber = pagination.PageNumber,
                        PageSize = pagination.PageSize,
                        TotalRow = totalRow
                    },
                    Data = feedbacks
                };
            }
            return null!;
        }

        public async Task<FeedBackViewModel> CreateFeedBackForDriver(Guid customerId, FeedBackCreateModel model)
        {
            var driver = await _driverRepository.GetMany(driver => driver.AccountId.Equals(model.DriverId)).FirstOrDefaultAsync();
            var driverFeedBack = await _feedBackRepository.GetMany(feedback => feedback.DriverId.Equals(model.DriverId)).ToListAsync();
            var feedBack = new FeedBack
            {
                Id = Guid.NewGuid(),
                DriverId = model.DriverId,
                Content = model.Content,
                Star = model.Star,
                CreateAt = DateTime.UtcNow.AddHours(7),
                CustomerId = customerId,
                OrderId = model.OrderId,
            };
            _feedBackRepository.Add(feedBack);
            var rates = driverFeedBack.Select(feedback => feedback.Star).ToList();
            rates.Add(model.Star);
            var rate = TinhTrungBinhCong(rates);
            driver!.Star = rate;
            _driverRepository.Update(driver);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetFeedBack(feedBack.Id) : null!;
        }

        private double TinhTrungBinhCong(ICollection<int> mangSo)
        {
            int tong = 0;
            double trungBinhCong = 0.0;

            if (mangSo != null && mangSo.Count > 0)
            {
                foreach (int so in mangSo)
                {
                    tong += so;
                }
                trungBinhCong = (double)tong / mangSo.Count;
            }

            return trungBinhCong;
        }


        public async Task<FeedBackViewModel> CreateFeedBackForCar(Guid customerId, FeedBackCreateModel model)
        {
            var car = await _carRepository.GetMany(car => car.Id.Equals(model.CarId)).FirstOrDefaultAsync();
            var carFeedBack = await _feedBackRepository.GetMany(feedback => feedback.CarId.Equals(model.CarId)).ToListAsync();
            var feedBack = new FeedBack
            {
                Id = Guid.NewGuid(),
                CarId = model.CarId,
                Content = model.Content,
                Star = model.Star,
                CreateAt = DateTime.UtcNow.AddHours(7),
                CustomerId = customerId,
                OrderId = model.OrderId,
            };
            _feedBackRepository.Add(feedBack);
            var rates = carFeedBack.Select(feedback => feedback.Star).ToList();
            rates.Add(model.Star);
            var rate = TinhTrungBinhCong(rates);
            car!.Star = rate;
            _carRepository.Update(car);
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
