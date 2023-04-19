using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Car
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string LicensePlate { get; set; } = null!;

    public double Price { get; set; }

    public DateTime CreateAt { get; set; }

    public string? Description { get; set; }

    public Guid ModelId { get; set; }

    public Guid? LocationId { get; set; }

    public Guid? AdditionalChargeId { get; set; }

    public Guid? DriverId { get; set; }

    public Guid? CarOwnerId { get; set; }

    public Guid? ShowroomId { get; set; }

    public int Rented { get; set; }

    public TimeSpan ReceiveStartTime { get; set; }

    public TimeSpan ReceiveEndTime { get; set; }

    public TimeSpan ReturnStartTime { get; set; }

    public TimeSpan ReturnEndTime { get; set; }

    public double? Star { get; set; }

    public bool IsTracking { get; set; }

    public string Status { get; set; } = null!;

    public virtual AdditionalCharge? AdditionalCharge { get; set; }

    public virtual ICollection<CarCalendar> CarCalendars { get; } = new List<CarCalendar>();

    public virtual ICollection<CarFeature> CarFeatures { get; } = new List<CarFeature>();

    public virtual CarOwner? CarOwner { get; set; }

    public virtual ICollection<CarType> CarTypes { get; } = new List<CarType>();

    public virtual Driver? Driver { get; set; }

    public virtual ICollection<DriverCalendar> DriverCalendars { get; } = new List<DriverCalendar>();

    public virtual ICollection<FeedBack> FeedBacks { get; } = new List<FeedBack>();

    public virtual ICollection<Image> Images { get; } = new List<Image>();

    public virtual Location? Location { get; set; }

    public virtual Model Model { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();

    public virtual Showroom? Showroom { get; set; }
}
