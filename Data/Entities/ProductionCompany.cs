using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class ProductionCompany
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Car> Cars { get; } = new List<Car>();

    public virtual ICollection<Model> Models { get; } = new List<Model>();
}
