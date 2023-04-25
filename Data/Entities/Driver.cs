using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Driver
{
    public Guid AccountId { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string Phone { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string? AvatarUrl { get; set; }

    public double? Star { get; set; }

    public int? Finished { get; set; }

    public string? BankAccountNumber { get; set; }

    public string? BankName { get; set; }

    public Guid WalletId { get; set; }

    public Guid? LocationId { get; set; }

    public Guid? WishAreaId { get; set; }

    public int? MinimumTrip { get; set; }

    public string Status { get; set; } = null!;

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Car> Cars { get; } = new List<Car>();

    public virtual ICollection<DriverCalendar> DriverCalendars { get; } = new List<DriverCalendar>();

    public virtual ICollection<FeedBack> FeedBacks { get; } = new List<FeedBack>();

    public virtual ICollection<Image> Images { get; } = new List<Image>();

    public virtual Location? Location { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();

    public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();

    public virtual Wallet Wallet { get; set; } = null!;

    public virtual Location? WishArea { get; set; }
}
