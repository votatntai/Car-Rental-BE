using Data.Models.Get;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface ITransactionService
    {
        Task<ListViewModel<TransactionViewModel>> GetTransactions(Guid userId, PaginationRequestModel pagination);
    }
}
