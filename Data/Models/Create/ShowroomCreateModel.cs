namespace Data.Models.Create
{
    public class ShowroomCreateModel
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public LocationCreateModel Location { get; set; } = null!;
    }
}
