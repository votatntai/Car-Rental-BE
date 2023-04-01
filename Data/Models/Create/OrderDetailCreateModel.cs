namespace Data.Models.Create
{
    public class OrderDetailCreateModel
    {
        public Guid CarId { get; set; }

        public bool HasDriver { get; set; }

        public LocationCreateModel? DeliveryLocation { get; set; }

        public LocationCreateModel? PickUpLocation { get; set; }

        public DateTime DeliveryTime { get; set; }

        public DateTime PickUpTime { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
