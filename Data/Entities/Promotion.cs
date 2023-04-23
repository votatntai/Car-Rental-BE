using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Promotion
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public double Discount { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime ExpiryAt { get; set; }

    public int Quantity { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
