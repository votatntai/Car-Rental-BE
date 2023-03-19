namespace Data.Models.Views
{
    public class CarRegistrationCalendarViewModel
    {
        public string? Description { get; set; }

        public CalendarViewModel Calendar { get; set; } = null!;
    }
}
