using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Calendar
{
    public Guid Id { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public Guid? CarId { get; set; }

    public Guid? DriverId { get; set; }

    public virtual Car? Car { get; set; }

    public virtual Driver? Driver { get; set; }
}
