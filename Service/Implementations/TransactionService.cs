using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Data.Entities;
using Data.Models.Create;
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
                return transactions != null || transactions != null && transactions.Any()
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

        public async Task<bool> CreateTransactionForCarOwner(Guid carOwnerId, TransactionCreateModel model)
        {
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                CarOwnerId = carOwnerId,
                Status = model.Status,
                Amount = model.Amount,
                Type= model.Type,
                Description = model.Description,
                CreateAt = DateTime.UtcNow.AddHours(7),
            };
            _transactionRepository.Add(transaction);
            return await _unitOfWork.SaveChanges() > 0;
        }

        public async Task<bool> CreateTransactionForCustomer(Guid customerId, TransactionCreateModel model)
        {
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                Status = model.Status,
                Type= model.Type,
                Amount = model.Amount,
                Description = model.Description,
                CreateAt = DateTime.UtcNow.AddHours(7),
            };
            _transactionRepository.Add(transaction);
            return await _unitOfWork.SaveChanges() > 0;
        }

    }
}
