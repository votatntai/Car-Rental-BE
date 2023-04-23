namespace Data.Models.Update
{
    public class PromotionUpdateModel
    {
        public string? Name { get; set; } = null!;

        public string? Description { get; set; }

        public double? Discount { get; set; }

        public int? Quantity { get; set; }

        public DateTime? ExpiryAt { get; set; }
    }
}
