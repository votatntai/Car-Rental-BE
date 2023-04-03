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

    public string Model { get; set; } = null!;

    public string Chassis { get; set; } = null!;

    public string Location { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public Guid CarOwnerId { get; set; }

    public Guid? AdditionalChargeId { get; set; }

    public string? Description { get; set; }

    public bool Status { get; set; }

    public virtual AdditionalCharge? AdditionalCharge { get; set; }

    public virtual CarOwner CarOwner { get; set; } = null!;

    public virtual ICollection<CarRegistrationCalendar> CarRegistrationCalendars { get; } = new List<CarRegistrationCalendar>();

    public virtual ICollection<Image> Images { get; } = new List<Image>();
}
