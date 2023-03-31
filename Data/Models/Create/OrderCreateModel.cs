namespace Data.Models.Create
{
    public class OrderCreateModel
    {
        public int RentalTime { get; set; }

        public Guid? PromotionId { get; set; }

        public bool IsPaid { get; set; }

        public float UnitPrice { get; set; }

        public float DeliveryFee { get; set; }

        public float DeliveryDistance{ get; set; }

        public float Deposit { get; set; }

        public float Amount { get; set; }

        public string? Description { get; set; }

        public ICollection<OrderDetailCreateModel> OrderDetails { get; set; } = null!;
    }
}
