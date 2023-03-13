namespace Data.Models.Views
{
    public class NotificationViewModel
    {
        public Guid Id { get; set; }

        public string Message { get; set; } = null!;

        public Guid AccountId { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreateAt { get; set; }
    }
}
