namespace Data.Models.Create
{
    public class CarCreateModel
    {
        public string Name { get; set; } = null!;

        public string LicensePlate { get; set; } = null!;

        public double Price { get; set; }

        public string? Description { get; set; }

        public Guid ModelId { get; set; }

        public Guid CarOwnerId { get; set; }

        public TimeSpan? ReceiveStartTime { get; set; }

        public TimeSpan? ReceiveEndTime { get; set; }

        public TimeSpan? ReturnStartTime { get; set; }

        public TimeSpan? ReturnEndTime { get; set; }

        public LocationCreateModel Location { get; set; } = null!;

        public ICollection<CarCalendarCreateModel>? Calendars { get; set; } = null!;

        public AdditionalChargeCreateModel AdditionalCharge { get; set; } = null!;

        public Guid? RegistrationId { get; set; } = null!;

        public Guid? ShowroomId { get; set; } = null!;
    }
}
