using Data.Models.Views;

namespace Data.Models.Create
{
    public class NotificationCreateModel
    {
        public ICollection<Guid> UserIds { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Body { get; set; } = null!;

        public NotificationDataViewModel Data { get; set; } = null!;
    }
}
