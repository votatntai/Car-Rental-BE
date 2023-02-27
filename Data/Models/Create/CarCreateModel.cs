namespace Data.Models.Create
{
    public class CarCreateModel
    {
        public string Name { get; set; } = null!;

        public string LicensePlate { get; set; } = null!;

        public string TransmissionType { get; set; } = null!;

        public string FuelType { get; set; } = null!;

        public int Seater { get; set; }

        public double Price { get; set; }

        public string FuelConsumption { get; set; } = null!;

        public int YearOfManufacture { get; set; }

        public string? Description { get; set; }

        public Guid ProductionCompanyId { get; set; }

        public LocationCreateModel Location { get; set; } = null!;

        public RouteCreateModel Route { get; set; } = null!;
    }
}
