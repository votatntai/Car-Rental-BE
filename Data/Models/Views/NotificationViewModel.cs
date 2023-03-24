namespace Data.Models.Views
{
    public class NotificationViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string Body { get; set; } = null!;

        public string? Link{ get; set; }

        public Guid AccountId { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreateAt { get; set; }
    }
}
