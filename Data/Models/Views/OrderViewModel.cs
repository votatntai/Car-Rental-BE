namespace Data.Models.Views
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }

        public CustomerViewModel Customer { get; set; } = null!;

        public ICollection<OrderDetailViewModel> OrderDetails { get; set; } = null!;

        public int RentalTime { get; set; }

        public double Amount { get; set; }

        public double UnitPrice { get; set; }

        public double? DeliveryFee { get; set; }

        public double? DeliveryDistance { get; set; }

        public double Deposit { get; set; }

        public bool IsPaid { get; set; }

        public string Status { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime CreateAt{ get; set; }

        public PromotionViewModel? Promotion { get; set; }
    }
}
