using Utility.Enums;

namespace Data.Models.Update
{
    public class CarUpdateModel
    {
        public string? Name { get; set; }

        public string? LicensePlate { get; set; } = null!;

        public string? TransmissionType { get; set; } = null!;

        public string? FuelType { get; set; } = null!;

        public int? Seater { get; set; }

        public double? Price { get; set; }

        public Guid? DriverId { get; set; }

        public string? FuelConsumption { get; set; } = null!;

        public int? YearOfManufacture { get; set; }

        public string? Description { get; set; }

        public Guid? ProductionCompanyId { get; set; }

        public LocationUpdateModel? Location { get; set; }

        public ICollection<CarCalendarUpdateModel>? CarCalendars { get; set; }

        public AdditionalChargeUpdateModel? AdditionalCharge { get; set; }

        public CarStatus? Status { get; set; } = null!;
    }
}
