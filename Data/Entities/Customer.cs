using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Customer
{
    public Guid AccountId { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string Phone { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string? AvatarUrl { get; set; }

    public string? BankAccountNumber { get; set; }

    public string? BankName { get; set; }

    public bool IsLicenseValid { get; set; }

    public Guid WalletId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<FeedBack> FeedBacks { get; } = new List<FeedBack>();

    public virtual ICollection<Image> Images { get; } = new List<Image>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();

    public virtual Wallet Wallet { get; set; } = null!;
}
