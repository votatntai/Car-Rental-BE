namespace Data.Models.Update
{
    public class CarCalendarUpdateModel
    {
        public string? Description { get; set; }

        public CalendarUpdateModel Calendar { get; set; } = null!;
    }
}
