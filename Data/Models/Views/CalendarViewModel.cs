namespace Data.Models.Views
{
    public class CalendarViewModel
    {
        public Guid Id { get; set; }

        public string Weekday { get; set; } = null!;

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
    }
}
