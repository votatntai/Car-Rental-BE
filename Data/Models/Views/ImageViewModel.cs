namespace Data.Models.Views
{
    public class ImageViewModel
    {
        public Guid Id { get; set; }

        public string Url { get; set; } = null!;

        public string Type { get; set; } = null!;
    }
}
