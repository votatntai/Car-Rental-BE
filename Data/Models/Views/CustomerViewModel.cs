﻿namespace Data.Models.Views
{
    public class CustomerViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Address { get; set; }

        public string Phone { get; set; } = null!;

        public string Gender { get; set; } = null!;

        public string? AvatarUrl { get; set; }

        public string? BankAccountNumber { get; set; }

        public string? BankName { get; set; }

        public bool Status { get; set; }

        public bool IsLicenseValid { get; set; }

        public ICollection<ImageViewModel> Images { get; set; } = new List<ImageViewModel>();
    }
}
