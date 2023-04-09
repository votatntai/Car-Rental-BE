using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Update
{
    public class CarOwnerUpdateModel
    {
        public string? Name { get; set; } = null!;

        public string? Address { get; set; }

        public string? Phone { get; set; } = null!;

        public string? Password { get; set; } = null!;

        public string? Gender { get; set; } = null!;

        public string? BankAccountNumber { get; set; }

        public string? BankName { get; set; }

        public bool? Status { get; set; }

        public bool? IsAutoAcceptOrder { get; set; }
    }
}
