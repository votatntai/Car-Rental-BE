using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Data.Models.Get;
using Data.Models.Views;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Service.Implementations
{
    public class TransactionService : BaseService, ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private new readonly IMapper _mapper;
        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _transactionRepository = unitOfWork.Transactions;
            _mapper = mapper;
        }

        public async Task<ListViewModel<TransactionViewModel>> GetTransactions(Guid userId, PaginationRequestModel pagination)
        {
            var query = _transactionRepository.GetAll()
                .Where(t => t.CarOwnerId == userId || t.DriverId == userId || t.UserId == userId || t.CustomerId == userId);
            var transactions = await query.ProjectTo<TransactionViewModel>(_mapper.ConfigurationProvider)
                .OrderByDescending(t => t.CreateAt)
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize)
                .AsNoTracking()
                .ToListAsync();
            var totalRow = await query.AsNoTracking().CountAsync();
            return transactions != null && transactions.Any()
                ? new ListViewModel<TransactionViewModel>
                {
                    Pagination = new PaginationViewModel
                    {
                        PageNumber = pagination.PageNumber,
                        PageSize = pagination.PageSize,
                        TotalRow = totalRow
                    },
                    Data = transactions
                }
                : null!;
        }

    }
}
