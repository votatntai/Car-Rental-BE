using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class AdditionalCharge
{
    public Guid Id { get; set; }

    public int MaximumDistance { get; set; }

    public double DistanceSurcharge { get; set; }

    public double TimeSurcharge { get; set; }

    public double AdditionalDdistance { get; set; }

    public double AdditionalTime { get; set; }

    public virtual ICollection<Car> Cars { get; } = new List<Car>();
}
