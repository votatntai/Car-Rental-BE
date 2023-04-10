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

        public TimeSpan ReceiveStartTime { get; set; }

        public TimeSpan ReceiveEndTime { get; set; }

        public TimeSpan ReturnStartTime { get; set; }

        public TimeSpan ReturnEndTime { get; set; }

        public string Description { get; set; } = null!;

        public ICollection<ImageViewModel> Images { get; set; } = new List<ImageViewModel>();

        public virtual ICollection<FeedBackViewModel> FeedBacks { get; set; } = new List<FeedBackViewModel>();

        public virtual ICollection<CarFeatureViewModel> CarFeatures { get; set; } = new List<CarFeatureViewModel>();

        public virtual ICollection<CarTypeViewModel> CarTypes { get; set; } = new List<CarTypeViewModel>();

        public virtual ICollection<DriverCalendarViewModel> DriverCalendars { get; set; } = new List<DriverCalendarViewModel>();

        public ProductionCompanyViewModel ProductionCompany { get; set; } = null!;

        public CarModelViewModel Model { get; set; } = null!;

        public CarOwnerViewModel? CarOwner { get; set; } = null!;

        public DriverViewModel? Driver { get; set; } = null!;


        public LocationViewModel Location { get; set; } = null!;

        public AdditionalChargeViewModel AdditionalCharge { get; set; } = null!;

        public double Star { get; set; }

        public bool IsTracking { get; set; }

        public string Status { get; set; } = null!;

        public ShowroomViewModel? Showroom { get; set; } = null!;
    }
}
