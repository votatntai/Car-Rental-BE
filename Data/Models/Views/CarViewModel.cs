namespace Data.Models.Views
{
    public class CarViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string LicensePlate { get; set; } = null!;

        public double Price { get; set; }

        public DateTime CreateAt { get; set; }

        public int Rented { get; set; }

        public string Description { get; set; } = null!;

        public ProductionCompanyViewModel ProductionCompany { get; set; } = null!;

        public CarModelViewModel Model { get; set; } = null!;

        public LocationViewModel Location { get; set; } = null!;

        public AdditionalChargeViewModel AdditionalCharge { get; set; } = null!;

        public double Star { get; set; }

        public string Status { get; set; } = null!;
    }
}
