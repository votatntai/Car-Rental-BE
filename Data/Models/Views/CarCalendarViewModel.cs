using Data.Entities;

namespace Data.Models.Views
{
    public class CarCalendarViewModel
    {
        public string? Description { get; set; }

        public CalendarViewModel Calendar { get; set; } = null!;

    }
}
