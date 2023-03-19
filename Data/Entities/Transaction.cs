using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Transaction
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public Guid? DriverId { get; set; }

    public Guid? CustomerId { get; set; }

    public Guid? CarOwnerId { get; set; }

    public string Type { get; set; } = null!;

    public double Amount { get; set; }

    public string? Description { get; set; }

    public DateTime? CreateAt { get; set; }

    public string? Status { get; set; }

    public virtual CarOwner? CarOwner { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Driver? Driver { get; set; }

    public virtual User? User { get; set; }
}
