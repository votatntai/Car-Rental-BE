using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Notification
{
    public Guid Id { get; set; }

    public string Message { get; set; } = null!;

    public Guid AccountId { get; set; }

    public bool IsRead { get; set; }

    public DateTime CreateAt { get; set; }

    public virtual Account Account { get; set; } = null!;
}
