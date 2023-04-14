using AutoMapper;
using Data;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service;
using Service.Interfaces;
using Utility.Helpers.Models;

public class VNPayService : BaseService, IVNPayService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ITransactionRepository _transactionRepository;
    private new readonly IMapper _mapper;

    public VNPayService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
        _customerRepository = unitOfWork.Customer;
        _transactionRepository = unitOfWork.Transactions;
        _mapper = mapper;
    }

    public async Task<bool> AddRequest(Guid customerId, VnPayRequestModel model)
    {
        var transaction = new Transaction
        {
            Amount = model.Amount,
            CreateAt = DateTime.UtcNow.AddHours(7),
            CustomerId = customerId,
            Type = "Nạp tiền",
            Status = "Đang xử lý",
            Id = model.TxnRef,
        };
        _transactionRepository.Add(transaction);
        return await _unitOfWork.SaveChanges() > 0;
    }

    public async Task<bool> AddResponse(VnPayResponseModel model)
    {
        var transaction = await _transactionRepository.GetMany(transaction => transaction.Id.Equals(model.TxnRef)).FirstOrDefaultAsync();
        if (transaction != null)
        {
            var customer = await _customerRepository.GetMany(customer => customer.AccountId.Equals(transaction.CustomerId))
                .Include(customer => customer.Wallet).FirstOrDefaultAsync();
            customer!.Wallet.Balance += transaction.Amount;
            transaction.Status = "Thành công";
            _customerRepository.Update(customer);
            _transactionRepository.Update(transaction);
        }
        return await _unitOfWork.SaveChanges() > 0;
    }
}