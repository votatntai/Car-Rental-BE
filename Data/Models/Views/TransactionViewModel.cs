using Utility.Enums;

namespace Data.Models.Views
{
    public class TransactionViewModel
    {
        public Guid Id { get; set; }

        public String Type { get; set; }

        public double Amount { get; set; }

        public string? Description { get; set; }

        public DateTime? CreateAt { get; set; }

        public string? Status { get; set; }
    }
}
