namespace Data.Models.Create
{
    public class CarShowroomCreateModel
    {
        public string? Name { get; set; } = null!;

        public string LicensePlate { get; set; } = null!;

        public double Price { get; set; }

        public string? Description { get; set; }

        public Guid ModelId { get; set; }

        public Guid? ShowroomId { get; set; } = null!;
    }
}
