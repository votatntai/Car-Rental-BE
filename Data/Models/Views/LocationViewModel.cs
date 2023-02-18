namespace Data.Models.Views
{
    public class LocationViewModel
    {
        public Guid Id { get; set; }

        public string Longitude { get; set; } = null!;

        public string Latitude { get; set; } = null!;
    }
}
