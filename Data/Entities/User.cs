using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class User
{
    public Guid AccountId { get; set; }

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string? AvatarUrl { get; set; }

    public string Role { get; set; } = null!;

    public Guid WalletId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();

    public virtual Wallet Wallet { get; set; } = null!;
}
