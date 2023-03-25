namespace Data.Models.Views
{
    public class FeatureViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
