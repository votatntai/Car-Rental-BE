using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Account
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool Status { get; set; }

    public virtual ICollection<CarOwner> CarOwners { get; } = new List<CarOwner>();

    public virtual ICollection<Customer> Customers { get; } = new List<Customer>();

    public virtual ICollection<Driver> Drivers { get; } = new List<Driver>();

    public virtual ICollection<Notification> Notifications { get; } = new List<Notification>();

    public virtual ICollection<User> Users { get; } = new List<User>();
}
