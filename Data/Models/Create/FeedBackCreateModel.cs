using Data.Entities;

namespace Data.Models.Create
{
    public class FeedBackCreateModel
    {
        public Guid OrderId { get; set; }

        public Guid? CarId { get; set; }

        public Guid? DriverId { get; set; }

        public int Star { get; set; }

        public string? Content { get; set; }

    }
}
