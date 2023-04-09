using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Image
{
    public Guid Id { get; set; }

    public string Url { get; set; } = null!;

    public string Type { get; set; } = null!;

    public Guid? ShowroomId { get; set; }

    public Guid? CarId { get; set; }

    public Guid? CarRegistrationId { get; set; }

    public Guid? CustomerId { get; set; }

    public Guid? DriverId { get; set; }

    public virtual Car? Car { get; set; }

    public virtual CarRegistration? CarRegistration { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Driver? Driver { get; set; }

    public virtual Showroom? Showroom { get; set; }
}
