using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Calendar
{
    public Guid Id { get; set; }

    public string Weekday { get; set; } = null!;

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public virtual ICollection<CarCalendar> CarCalendars { get; } = new List<CarCalendar>();

    public virtual ICollection<CarRegistrationCalendar> CarRegistrationCalendars { get; } = new List<CarRegistrationCalendar>();

    public virtual ICollection<DriverCalendar> DriverCalendars { get; } = new List<DriverCalendar>();
}
