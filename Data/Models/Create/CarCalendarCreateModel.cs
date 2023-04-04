namespace Data.Models.Create
{
    public class CarCalendarCreateModel
    {
        public string? Description { get; set; }

        public CalendarCreateModel Calendar { get; set; } = null!;
    }
}
