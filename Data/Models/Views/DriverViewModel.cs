using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Views
{
    public class DriverViewModel
    {
        public Guid AccountId { get; set; }

        public string Name { get; set; } = null!;

        public string? Address { get; set; }

        public string Phone { get; set; } = null!;

        public string Gender { get; set; } = null!;

        public string? AvatarUrl { get; set; }

        public double? Star { get; set; }

        public string? BankAccountNumber { get; set; }

        public string? BankName { get; set; }

        public LocationViewModel? Location { get; set; }

        public string Status { get; set; } = null!;

        public bool AccountStatus { get; set; }

    }
}
