using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Views
{
    public class CustomerViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Address { get; set; }

        public string Phone { get; set; } = null!;

        public string Gender { get; set; } = null!;

        public string? AvartarUrl { get; set; }

        public string? BankAccountNumber { get; set; }

        public string? BankName { get; set; }

        public WalletViewModel Wallet { get; set; } = null!;
    }
}
