namespace Data.Models.Update
{
    public class DriverUpdateModel
    {
        public string? Name { get; set; } = null!;

        public string? Address { get; set; }

        public string? Phone { get; set; } = null!;

        public string? Gender { get; set; } = null!;

        public string? BankAccountNumber { get; set; }

        public string? BankName { get; set; }

        public string? Password { get; set; }

        public string? Status { get; set; }

        public bool? AccountStatus { get; set; }
    }
}
