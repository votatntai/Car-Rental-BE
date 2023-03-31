using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Order
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public int RentalTime { get; set; }

    public double UnitPrice { get; set; }

    public double? DeliveryFee { get; set; }

    public double? DeliveryDistance { get; set; }

    public double Deposit { get; set; }

    public double Amount { get; set; }

    public Guid? PromotionId { get; set; }

    public bool IsPaid { get; set; }

    public string Status { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreateAt { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<FeedBack> FeedBacks { get; } = new List<FeedBack>();

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();

    public virtual Promotion? Promotion { get; set; }
}
