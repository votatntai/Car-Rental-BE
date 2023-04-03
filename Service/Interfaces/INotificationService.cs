using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface INotificationService
    {
        Task<ListViewModel<NotificationViewModel>> GetNotifications(Guid userId, PaginationRequestModel pagination);
        Task<NotificationViewModel> GetNotification(Guid id);
        Task<bool> SendNotification(ICollection<Guid> userId, NotificationCreateModel model);
        Task<NotificationViewModel> UpdateNotification(Guid id, NotificationUpdateModel model);
        Task<bool> MakeAsRead(Guid userId);
        Task<bool> DeleteNotification(Guid id);
    }
}
