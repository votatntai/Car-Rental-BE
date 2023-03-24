using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Account
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool Status { get; set; }

    public virtual CarOwner? CarOwner { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<DeviceToken> DeviceTokens { get; } = new List<DeviceToken>();

    public virtual Driver? Driver { get; set; }

    public virtual ICollection<Notification> Notifications { get; } = new List<Notification>();

    public virtual User? User { get; set; }
}
