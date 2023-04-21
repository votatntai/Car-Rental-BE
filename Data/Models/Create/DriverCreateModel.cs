namespace Data.Models.Create
{
    public class DriverCreateModel
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string BankName { get; set; } = null!;

        public string BankAccountNumber { get; set; } = null!;

        public string? Address { get; set; }

        public string Phone { get; set; } = null!;

        public string Gender { get; set; } = null!;

        public LocationCreateModel Location { get; set; } = null!;
    }
}
