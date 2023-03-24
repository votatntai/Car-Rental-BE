using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Wallet
{
    public Guid Id { get; set; }

    public double Balance { get; set; }

    public string Status { get; set; } = null!;

    public virtual CarOwner? CarOwner { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Driver? Driver { get; set; }

    public virtual User? User { get; set; }
}
