using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;
using Data.Repositories.Interfaces;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using Data.Models.Create;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;

namespace Service.Implementations
{
    public class NotificationService : BaseService, INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IDeviceTokenRepository _deviceTokenRepository;
        private new readonly IMapper _mapper;
        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _notificationRepository = unitOfWork.Notification;
            _deviceTokenRepository = unitOfWork.DeviceToken;
            _mapper = mapper;
        }

        public async Task<ListViewModel<NotificationViewModel>> GetNotifications(Guid userId, PaginationRequestModel pagination)
        {
            var query = _notificationRepository.GetMany(notification =>
                    notification.Account.Customer != null ? notification.Account.Customer.AccountId.Equals(userId) :
                    notification.Account.CarOwner != null ? notification.Account.CarOwner.AccountId.Equals(userId) :
                    notification.Account.Driver != null ? notification.Account.Driver.AccountId.Equals(userId) :
                    notification.Account.User != null ? notification.Account.User.AccountId.Equals(userId) :
                    false);
            var notifications = await query
                .OrderByDescending(notification => notification.CreateAt)
                .ProjectTo<NotificationViewModel>(_mapper.ConfigurationProvider)
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize)
                .AsNoTracking()
                .ToListAsync();

            var totalRow = await query.AsNoTracking().CountAsync();

            return notifications != null
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
            var notifications = await _notificationRepository.GetMany(notification => notification.AccountId.Equals(userId)).ToListAsync();
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }
            _notificationRepository.UpdateRange(notifications);
            var result = await _unitOfWork.SaveChanges();
            return result > 0;
        }

        public async Task<bool> SendNotification(ICollection<Guid> userIds, NotificationCreateModel model)
        {
            var deviceTokens = await _deviceTokenRepository.GetMany(dvt => userIds.Contains(dvt.AccountId))
                .Select(dvt => dvt.Token).ToListAsync();
            var now = DateTime.UtcNow.AddHours(7);
            foreach (var userId in userIds)
            {
                var notification = new Data.Entities.Notification
                {
                    Id = Guid.NewGuid(),
                    AccountId = userId,
                    CreateAt = now,
                    Body = model.Body,
                    IsRead = false,
                    Type = model.Data.Type,
                    Link = model.Data.Link,
                    Title = model.Title,
                };
                _notificationRepository.Add(notification);
            }
            var result = await _unitOfWork.SaveChanges();
            if (result > 0)
            {
                if (deviceTokens.Any())
                {
                    var messageData = new Dictionary<string, string>{
                            { "type", model.Data.Type ?? "" },
                            { "link", model.Data.Link ?? "" },
                            { "createAt", now.ToString() },
                        };
                    var message = new MulticastMessage()
                    {
                        Notification = new FirebaseAdmin.Messaging.Notification
                        {
                            Title = model.Title,
                            Body = model.Body,
                        },
                        Data = messageData,
                        Tokens = deviceTokens
                    };
                    var app = FirebaseApp.DefaultInstance;
                    if (FirebaseApp.DefaultInstance == null)
                    {
                        app = FirebaseApp.Create(new AppOptions()
                        {
                            Credential = GoogleCredential.FromFile("cloud-storage.json")
                        });
                    }
                    FirebaseMessaging messaging = FirebaseMessaging.GetMessaging(app);
                    await messaging.SendMulticastAsync(message);
                }
            }
            return true;
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
