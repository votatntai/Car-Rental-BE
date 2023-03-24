using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Data.Entities;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;
using Data.Repositories.Implementations;
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

        public async Task<ListViewModel<NotificationViewModel>> GetNotifications(Guid userId, PaginationRequestModel pagination)
        {
            var query = _notificationRepository.GetMany(notification =>
                    notification.Account.Customer != null ? notification.Account.Customer.Id.Equals(userId) :
                    notification.Account.CarOwner != null ? notification.Account.CarOwner.Id.Equals(userId) :
                    notification.Account.Driver != null ? notification.Account.Driver.Id.Equals(userId) :
                    notification.Account.User != null ? notification.Account.User.Id.Equals(userId) :
                    false);
            var notifications = await query
                .ProjectTo<NotificationViewModel>(_mapper.ConfigurationProvider)
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize)
                .AsNoTracking()
                .ToListAsync();

            var totalRow = await query.AsNoTracking().CountAsync();

            return notifications != null && notifications.Any()
                ? new ListViewModel<NotificationViewModel>
                {
                    Pagination = new PaginationViewModel
                    {
                        PageNumber = pagination.PageNumber,
                        PageSize = pagination.PageSize,
                        TotalRow = totalRow
                    },
                    Data = notifications
                }
                : null!;
        }

        public async Task<NotificationViewModel> GetNotification(Guid id)
        {
            return await _notificationRepository.GetMany(notification => notification.Id.Equals(id))
                .ProjectTo<NotificationViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? null!;
        }

        public async Task<NotificationViewModel> UpdateNotification(Guid id, NotificationUpdateModel model)
        {
            var notification = await _notificationRepository.GetMany(notification => notification.Id.Equals(id)).FirstOrDefaultAsync();
            if (notification == null)
            {
                return null!;
            }
            notification.IsRead = model.IsRead;
            _notificationRepository.Update(notification);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetNotification(id) : null!;
        }

        public async Task<bool> MakeAsRead(Guid userId)
        {
            var notifications = await _notificationRepository.GetMany(notification =>
                notification.Account.Customer != null ? notification.Account.Customer.Id.Equals(userId) :
                notification.Account.CarOwner != null ? notification.Account.CarOwner.Id.Equals(userId) :
                notification.Account.Driver != null ? notification.Account.Driver.Id.Equals(userId) :
                notification.Account.User != null ? notification.Account.User.Id.Equals(userId) :
                false).ToListAsync();
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }
            _notificationRepository.UpdateRange(notifications);
            var result = await _unitOfWork.SaveChanges();
            return result > 0;
        }

        public async Task<bool> DeleteNotification(Guid id)
        {
            var notification = await _notificationRepository.GetMany(notification => notification.Id.Equals(id)).FirstOrDefaultAsync();
            if (notification == null)
            {
                return false;
            }
            _notificationRepository.Remove(notification);
            var result = await _unitOfWork.SaveChanges();
            return result > 0;
        }
    }
}
