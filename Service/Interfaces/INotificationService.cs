using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface INotificationService
    {
        Task<ListViewModel<NotificationViewModel>> GetNotifications(Guid userId, PaginationRequestModel pagination);
        Task<NotificationViewModel> GetNotification(Guid id);
        Task<NotificationViewModel> UpdateNotification(Guid id, NotificationUpdateModel model);
        Task<NotificationViewModel> MakeAsRead(Guid id);
        Task<bool> DeleteNotification(Guid id);
    }
}
