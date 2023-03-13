namespace Data.Models.Create
{
    public class CarCreateModel
    {
        public string Name { get; set; } = null!;

        public string LicensePlate { get; set; } = null!;

        public double Price { get; set; }

        public string? Description { get; set; }

        public Guid ModelId { get; set; }

        public TimeSpan ReceiveTime { get; set; }

        public TimeSpan ReturnTime { get; set; }

        public LocationCreateModel Location { get; set; } = null!;

        public AdditionalChargeCreateModel AdditionalCharge { get; set; } = null!;
    }
}
