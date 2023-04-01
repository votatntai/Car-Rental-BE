using Data.Models.Views;

namespace Data.Models.Create
{
    public class NotificationCreateModel
    {
        public Guid UserId { get; set; }

        public string Title { get; set; } = null!;

        public string Body { get; set; } = null!;

        public NotificationDataViewModel Data { get; set; } = null!;
    }
}
