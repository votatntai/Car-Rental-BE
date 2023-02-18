using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Car
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string LicensePlate { get; set; } = null!;

    public string TransmissionType { get; set; } = null!;

    public string FuelType { get; set; } = null!;

    public int Seater { get; set; }

    public double Price { get; set; }

    public string FuelConsumption { get; set; } = null!;

    public int YearOfManufacture { get; set; }

    public DateTime CreateAt { get; set; }

    public string? Description { get; set; }

    public Guid ProductionCompanyId { get; set; }

    public Guid? LocationId { get; set; }

    public Guid? RouteId { get; set; }

    public int Rented { get; set; }

    public double? Star { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Calendar> Calendars { get; } = new List<Calendar>();

    public virtual ICollection<CarFeature> CarFeatures { get; } = new List<CarFeature>();

    public virtual ICollection<CarType> CarTypes { get; } = new List<CarType>();

    public virtual ICollection<FeedBack> FeedBacks { get; } = new List<FeedBack>();

    public virtual ICollection<Image> Images { get; } = new List<Image>();

    public virtual Location? Location { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();

    public virtual ProductionCompany ProductionCompany { get; set; } = null!;

    public virtual Route? Route { get; set; }
}
