using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class FeedBack
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid CustomerId { get; set; }

    public Guid? CarId { get; set; }

    public Guid? DriverId { get; set; }

    public int Star { get; set; }

    public DateTime CreateAt { get; set; }

    public string? Content { get; set; }

    public virtual Car? Car { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Driver? Driver { get; set; }

    public virtual Order Order { get; set; } = null!;
}
