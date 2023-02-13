using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Wallet
{
    public Guid Id { get; set; }

    public double Balance { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<CarOwner> CarOwners { get; } = new List<CarOwner>();

    public virtual ICollection<Customer> Customers { get; } = new List<Customer>();

    public virtual ICollection<Driver> Drivers { get; } = new List<Driver>();

    public virtual ICollection<User> Users { get; } = new List<User>();
}
