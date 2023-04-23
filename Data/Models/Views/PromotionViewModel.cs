namespace Data.Models.Views
{
    public class PromotionViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public double Discount { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime ExpiryAt { get; set; }

        public int Quantity { get; set; }
    }
}
