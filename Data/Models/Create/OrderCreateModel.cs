namespace Data.Models.Create
{
    public class OrderCreateModel
    {
        public DateTime RentalTime { get; set; }

        public Guid? PromotionId { get; set; }

        public bool IsPaid { get; set; }

        public string? Description { get; set; }

        public ICollection<OrderDetailCreateModel> OrderDetails { get; set; } = null!;
    }
}
