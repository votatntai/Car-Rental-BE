namespace Data.Models.Views
{
    public class DriverViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Address { get; set; }

        public string Phone { get; set; } = null!;

        public string Gender { get; set; } = null!;

        public string? AvatarUrl { get; set; }

        public double? Star { get; set; }

        public int? Finished { get; set; }

        public string? BankAccountNumber { get; set; }

        public string? BankName { get; set; }

        public LocationViewModel? Location { get; set; }

        public LocationViewModel? WishArea { get; set; }

        public int MinimumTrip { get; set; }

        public string Status { get; set; } = null!;

        public bool AccountStatus { get; set; }

    }
}
