using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class CarFeature
{
    public Guid CarId { get; set; }

    public Guid FeatureId { get; set; }

    public string? Description { get; set; }

    public virtual Car Car { get; set; } = null!;

    public virtual Feature Feature { get; set; } = null!;
}
