namespace Data.Models.Views
{
    public class CarViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string LicensePlate { get; set; } = null!;

        public string TransmissionType { get; set; } = null!;

        public string FuelType { get; set; } = null!;

        public int Seater { get; set; }

        public double Price { get; set; }

        public string FuelConsumption { get; set; } = null!;

        public int YearOfManufacture { get; set; }

        public DateTime CreateAt { get; set; }

        public string Description { get; set; } = null!;

        public ProductionCompanyViewModel ProductionCompany { get; set; } = null!;

        public LocationViewModel Location { get; set; } = null!;

        public RouteViewModel Route { get; set; } = null!;

        public int Rented { get; set; }

        public double Star { get; set; }

        public string Status { get; set; } = null!;
    }
}
