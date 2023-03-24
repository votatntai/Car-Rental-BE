using Data.Models.Views;

namespace Service.Interfaces
{
    public interface IWalletService
    {
        Task<WalletViewModel> GetWallet(Guid id);
    }
}
