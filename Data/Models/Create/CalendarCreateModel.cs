using Utility.Enums;

namespace Data.Models.Create
{
    public class CalendarCreateModel
    {
        public Weekday Weekday { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
    }
}
