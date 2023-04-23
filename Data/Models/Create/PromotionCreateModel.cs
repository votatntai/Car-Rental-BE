namespace Data.Models.Create
{
    public class PromotionCreateModel
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public double Discount { get; set; }

        public int Quantity { get; set; }

        public DateTime ExpiryAt { get; set; }
    }
}
