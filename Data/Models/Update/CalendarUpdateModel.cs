namespace Data.Models.Update
{
    public class CalendarUpdateModel
    {
        public Guid Id { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }
    }
}
