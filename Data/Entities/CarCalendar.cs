using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class CarCalendar
{
    public Guid CalendarId { get; set; }

    public Guid CarId { get; set; }

    public string? Description { get; set; }

    public virtual Calendar Calendar { get; set; } = null!;

    public virtual Car Car { get; set; } = null!;
}
