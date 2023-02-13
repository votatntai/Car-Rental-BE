using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Order
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public DateTime RentalTime { get; set; }

    public double Amount { get; set; }

    public Guid? PromotionId { get; set; }

    public bool IsPaid { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<ExpensesIncurred> ExpensesIncurreds { get; } = new List<ExpensesIncurred>();

    public virtual Promotion? Promotion { get; set; }
}
