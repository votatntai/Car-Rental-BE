using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Showroom
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public Guid? LocationId { get; set; }

    public virtual ICollection<Car> Cars { get; } = new List<Car>();

    public virtual ICollection<Image> Images { get; } = new List<Image>();

    public virtual Location? Location { get; set; }
}
