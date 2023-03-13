using Data.Models.Get;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface INotificationService
    {
        Task<ListViewModel<NotificationViewModel>> GetNotifications(NotificationFilterModel filter, PaginationRequestModel pagination);
    }
}
