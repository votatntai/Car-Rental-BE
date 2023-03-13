using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class CarRegistrationCalendar
{
    public Guid CalendarId { get; set; }

    public Guid CarRegistrationId { get; set; }

    public string? Description { get; set; }

    public virtual Calendar Calendar { get; set; } = null!;

    public virtual CarRegistration CarRegistration { get; set; } = null!;
}
