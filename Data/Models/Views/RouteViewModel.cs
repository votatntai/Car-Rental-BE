namespace Data.Models.Views
{
    public class RouteViewModel
    {
        public Guid Id { get; set; }

        public int MaximumDistance { get; set; }

        public double DistanceSurcharge { get; set; }

        public double TimeSurcharge { get; set; }
    }
}
