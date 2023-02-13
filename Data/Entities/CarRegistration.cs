using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class CarRegistration
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

    public string ProductionCompany { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string Type { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Image> Images { get; } = new List<Image>();
}
