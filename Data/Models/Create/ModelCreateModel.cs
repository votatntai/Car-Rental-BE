namespace Data.Models.Create
{
    public class ModelCreateModel
    {
        public string Name { get; set; } = null!;

        public double CellingPrice { get; set; }

        public double FloorPrice { get; set; }

        public int Seater { get; set; }

        public string Chassis { get; set; } = null!;

        public int YearOfManufacture { get; set; }

        public string TransmissionType { get; set; } = null!;

        public string FuelType { get; set; } = null!;

        public string FuelConsumption { get; set; } = null!;

        public Guid ProductionCompanyId { get; set; }
    }
}
