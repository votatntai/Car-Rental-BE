namespace Data.Models.Views
{
    public class WalletViewModel
    {
        public Guid Id { get; set; }

        public double Balance { get; set; }

        public string Status { get; set; } = null!;
    }
}
