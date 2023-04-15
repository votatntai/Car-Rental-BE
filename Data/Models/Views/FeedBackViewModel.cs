namespace Data.Models.Views
{
    public class FeedBackViewModel
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public Guid CustomerId { get; set; }

        public Guid DriverId { get; set; }

        public int Star { get; set; }

        public DateTime CreateAt { get; set; }

        public string? Content { get; set; }
    }
}
