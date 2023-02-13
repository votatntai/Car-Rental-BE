using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class CarType
{
    public Guid CarId { get; set; }

    public Guid TypeId { get; set; }

    public string? Description { get; set; }

    public virtual Car Car { get; set; } = null!;

    public virtual Type Type { get; set; } = null!;
}
