namespace Data.Models.Views
{
    public class CarModelViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string TransmissionType { get; set; } = null!;

        public string FuelType { get; set; } = null!;

        public string Chassis { get; set; } = null!;

        public int Seater { get; set; }

        public int YearOfManufacture { get; set; }

        public string FuelConsumption { get; set; } = null!;

        public double CellingPrice { get; set; }

        public double FloorPrice { get; set; }

        public ProductionCompanyViewModel ProductionCompany { get; set; } = null!;
    }
}
