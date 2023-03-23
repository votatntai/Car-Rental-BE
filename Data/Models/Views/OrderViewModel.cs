namespace Data.Models.Views
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }

        public CustomerViewModel Customer { get; set; } = null!;

        public OrderDetailViewModel OrderDetail { get; set; } = null!;

        public DateTime RentalTime { get; set; }

        public double Amount { get; set; }

        public bool IsPaid { get; set; }

        public string Status { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime CreateAt{ get; set; }

    }
}
