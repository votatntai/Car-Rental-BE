using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Views;

namespace Service.Interfaces
{
    public interface ITransactionService
    {
        Task<ListViewModel<TransactionViewModel>> GetTransactions(Guid userId, PaginationRequestModel pagination);
        Task<bool> CreateTransactionForCarOwner(Guid carOwnerId, TransactionCreateModel model);
        Task<bool> CreateTransactionForCustomer(Guid customerId, TransactionCreateModel model);
    }
}
