using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Data.Entities;
using Data.Models.Get;
using Data.Models.Views;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Service.Implementations
{
    public class NotificationService : BaseService, INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private new readonly IMapper _mapper;
        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _notificationRepository = unitOfWork.Notification;
            _mapper = mapper;
        }

        public async Task<ListViewModel<NotificationViewModel>> GetNotifications(NotificationFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _notificationRepository.GetMany(notification => notification.AccountId.Equals(filter.AccountId))
                .ProjectTo<NotificationViewModel>(_mapper.ConfigurationProvider);
            var notifications = await query.Skip(pagination.PageNumber * pagination.PageSize).Take(pagination.PageSize).AsNoTracking().ToListAsync();
            var totalRow = await query.AsNoTracking().CountAsync();
            if (notifications != null || notifications != null && notifications.Any())
            {
                return new ListViewModel<NotificationViewModel>
                {
                    Pagination = new PaginationViewModel
                    {
                        PageNumber = pagination.PageNumber,
                        PageSize = pagination.PageSize,
                        TotalRow = totalRow
                    },
                    Data = notifications
                };
            }
            return null!;

        }
    }
}
