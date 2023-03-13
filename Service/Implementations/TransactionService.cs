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

        public async Task<ListViewModel<TransactionViewModel>> GetTransactions(TransactionFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _transactionRepository.GetAll();
            if (filter.CarOwnerId != null)
            {
                query = _transactionRepository.GetMany(transaction => transaction.CarOwnerId.Equals(filter.CarOwnerId));
            }
            if (filter.DriverId != null)
            {
                query = _transactionRepository.GetMany(transaction => transaction.DriverId.Equals(filter.DriverId));
            }
            if (filter.CustomerId != null)
            {
                query = _transactionRepository.GetMany(transaction => transaction.CustomerId.Equals(filter.CustomerId));
            }
            if (filter.UserId != null)
            {
                query = _transactionRepository.GetMany(transaction => transaction.UserId.Equals(filter.UserId));
            }
            var transactions = await query.ProjectTo<TransactionViewModel>(_mapper.ConfigurationProvider)
                .Skip(pagination.PageNumber * pagination.PageSize).Take(pagination.PageSize).AsNoTracking().ToListAsync();
            var totalRow = await query.AsNoTracking().CountAsync();
            if (transactions != null || transactions != null && transactions.Any())
            {
                return new ListViewModel<TransactionViewModel>
                {
                    Pagination = new PaginationViewModel
                    {
                        PageNumber = pagination.PageNumber,
                        PageSize = pagination.PageSize,
                        TotalRow = totalRow
                    },
                    Data = transactions
                };
            }   
                return null!;
        }
    }
}
