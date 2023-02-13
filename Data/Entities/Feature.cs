using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Feature
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<CarFeature> CarFeatures { get; } = new List<CarFeature>();
}
