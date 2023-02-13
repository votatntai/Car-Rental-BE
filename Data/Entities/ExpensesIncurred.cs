using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class ExpensesIncurred
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public double Amount { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Image> Images { get; } = new List<Image>();

    public virtual Order Order { get; set; } = null!;
}
