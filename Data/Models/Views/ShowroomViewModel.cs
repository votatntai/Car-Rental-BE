namespace Data.Models.Views
{
    public class ShowroomViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int NumberOfCar { get; set; }

        public LocationViewModel? Location { get; set; }
    }
}
