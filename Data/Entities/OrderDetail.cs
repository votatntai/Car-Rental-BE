using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class OrderDetail
{
    public Guid Id { get; set; }

    public Guid? OrderId { get; set; }

    public Guid? CarId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public Guid? DeliveryLocationId { get; set; }

    public Guid? PickUpLocationId { get; set; }

    public DateTime DeliveryTime { get; set; }

    public DateTime PickUpTime { get; set; }

    public Guid? DriverId { get; set; }

    public virtual Car? Car { get; set; }

    public virtual Location? DeliveryLocation { get; set; }

    public virtual Driver? Driver { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Location? PickUpLocation { get; set; }
}
