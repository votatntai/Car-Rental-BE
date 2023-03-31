namespace Data.Models.Views
{
    public class AdditionalChargeViewModel
    {
        public Guid Id { get; set; }

        public int MaximumDistance { get; set; }

        public double DistanceSurcharge { get; set; }

        public double TimeSurcharge { get; set; }

        public double AdditionalDistance { get; set; }

        public double AdditionalTime { get; set; }
    }
}
