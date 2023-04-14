using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class CarOwner
{
    public Guid AccountId { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string Phone { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string? AvatarUrl { get; set; }

    public string? BankAccountNumber { get; set; }

    public string? BankName { get; set; }

    public bool IsAutoAcceptOrder { get; set; }

    public Guid WalletId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<CarRegistration> CarRegistrations { get; } = new List<CarRegistration>();

    public virtual ICollection<Car> Cars { get; } = new List<Car>();

    public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();

    public virtual Wallet Wallet { get; set; } = null!;
}
