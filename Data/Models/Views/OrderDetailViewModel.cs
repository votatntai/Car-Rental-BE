namespace Data.Models.Views
{
    public class OrderDetailViewModel
    {
        public Guid Id { get; set; }

        public CarViewModel Car { get; set; } = null!;

        public LocationViewModel? DeliveryLocation { get; set; }

        public LocationViewModel? PickUpLocation { get; set; }

        public DateTime DeliveryTime { get; set; }

        public DateTime PickUpTime { get; set; }

        public DriverViewModel? Driver { get; set; }
    }
}
