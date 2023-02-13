using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Type
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<CarType> CarTypes { get; } = new List<CarType>();
}
