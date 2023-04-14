using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Location
{
    public Guid Id { get; set; }

    public double Longitude { get; set; }

    public double Latitude { get; set; }

    public virtual ICollection<Car> Cars { get; } = new List<Car>();

    public virtual ICollection<Driver> DriverLocations { get; } = new List<Driver>();

    public virtual ICollection<Driver> DriverWishAreas { get; } = new List<Driver>();

    public virtual ICollection<OrderDetail> OrderDetailDeliveryLocations { get; } = new List<OrderDetail>();

    public virtual ICollection<OrderDetail> OrderDetailPickUpLocations { get; } = new List<OrderDetail>();

    public virtual ICollection<Showroom> Showrooms { get; } = new List<Showroom>();
}
