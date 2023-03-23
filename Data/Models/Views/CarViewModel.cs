using Data.Entities;

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

        public TimeSpan ReceiveTime  { get; set; }

        public TimeSpan ReturnTime { get; set; }

        public string Description { get; set; } = null!;

        public ICollection<ImageViewModel> Images { get; set; } = null!;

        public virtual ICollection<FeedBackViewModel> FeedBacks { get; set; } = null!;

        public virtual ICollection<CarCalendarViewModel> CarCalendars { get; set; } = null!;

        public virtual ICollection<CarFeatureViewModel> CarFeatures { get; set; } = null!;

        public virtual ICollection<CarTypeViewModel> CarTypes { get; set; } = null!;

        public ProductionCompanyViewModel ProductionCompany { get; set; } = null!;

        public CarModelViewModel Model { get; set; } = null!;

        public CarOwnerViewModel? CarOwner { get; set; } = null!;

        public DriverViewModel? Driver { get; set; } = null!;

        public virtual ICollection<DriverCalendarViewModel> DriverCalendars { get; set; } = null!;

        public LocationViewModel Location { get; set; } = null!;

        public AdditionalChargeViewModel AdditionalCharge { get; set; } = null!;

        public double Star { get; set; }

        public string Status { get; set; } = null!;

        public ShowroomViewModel? Showroom { get; set; } = null!;
    }
}
