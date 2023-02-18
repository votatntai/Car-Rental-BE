using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Location
{
    public Guid Id { get; set; }

    public string Longitude { get; set; } = null!;

    public string Latitude { get; set; } = null!;

    public virtual ICollection<Car> Cars { get; } = new List<Car>();

    public virtual ICollection<Driver> Drivers { get; } = new List<Driver>();

    public virtual ICollection<OrderDetail> OrderDetailDeliveryLocations { get; } = new List<OrderDetail>();

    public virtual ICollection<OrderDetail> OrderDetailPickUpLocations { get; } = new List<OrderDetail>();

    public virtual ICollection<Showroom> Showrooms { get; } = new List<Showroom>();
}
