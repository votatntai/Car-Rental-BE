namespace Data.Models.Create
{
    public class CarRegistrationCreateModel
    {
        public string? Name { get; set; }

        public string LicensePlate { get; set; } = null!;

        public string TransmissionType { get; set; } = null!;

        public string FuelType { get; set; } = null!;

        public string Model { get; set; } = null!;

        public int Seater { get; set; }

        public double Price { get; set; }

        public string FuelConsumption { get; set; } = null!;

        public string Chassis { get; set; } = null!;

        public int YearOfManufacture { get; set; }

        public string ProductionCompany { get; set; } = null!;

        public string Location { get; set; } = null!;

        public string Type { get; set; } = null!;

        public string? Description { get; set; }

        public AdditionalChargeCreateModel AdditionalCharge { get; set; } = null!;
    }
}
