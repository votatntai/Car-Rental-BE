namespace Data.Models.Views
{
    public class CarFeatureViewModel
    {
        public string? Description { get; set; }

        public FeatureViewModel Feature { get; set; } = null!;
    }
}
