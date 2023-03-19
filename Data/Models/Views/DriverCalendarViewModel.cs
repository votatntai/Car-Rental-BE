using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Views
{
    public class DriverCalendarViewModel
    {
        public string? Description { get; set; }

        public virtual CalendarViewModel Calendar { get; set; } = null!;
    }
}
