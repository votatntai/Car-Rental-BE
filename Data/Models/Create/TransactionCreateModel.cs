namespace Data.Models.Create
{
    public class TransactionCreateModel
    {
        public string Type { get; set; } = null!;

        public double Amount { get; set; }

        public string? Description { get; set; }

        public string? Status { get; set; }
    }
}
