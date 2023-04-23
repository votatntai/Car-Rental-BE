using Microsoft.AspNetCore.Http;

namespace Data.Models.Update
{
    public class CustomerUpdateModel
    {
        public string? Name { get; set; } = null!;

        public string? Address { get; set; }

        public string? Phone { get; set; } = null!;

        public string? Gender { get; set; } = null!;

        public string? BankAccountNumber { get; set; }

        public string? BankName { get; set; }

        public string? Password { get; set; }

        public bool? IsLicenseValid { get; set; }

        public string? Description { get; set; }

        public bool? Status { get; set; }
    }
}