using Data;
using Data.Entities;
using Data.Models.Create;
using Data.Models.Views;
using Data.Repositories.Interfaces;
using Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Utility.Constants;
using Data.Models.Update;

namespace Service.Implementations
{
    public class CustomerService : BaseService, ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IWalletRepository _walletRepository;

        public CustomerService(IUnitOfWork unitOfWork, CarRentalContext context) : base(unitOfWork)
        {
            _customerRepository = unitOfWork.Customer;
            _accountRepository = unitOfWork.Account;
            _walletRepository = unitOfWork.Wallet;
        }

        public async Task<CustomerViewModel> GetCustomer(Guid id)
        {
            return await _customerRepository.GetMany(customer => customer.Id.Equals(id))
                .Select(customer => new CustomerViewModel
                {
                    Id = customer.Id,
                    Address = customer.Address,
                    AvartarUrl = customer.AvartarUrl,
                    BankAccountNumber = customer.BankAccountNumber,
                    BankName = customer.BankName,
                    Gender = customer.Gender,
                    Name = customer.Name,
                    Phone = customer.Phone,
                    Wallet = new WalletViewModel()
                    {
                        Id = customer.Wallet.Id,
                        Balance = customer.Wallet.Balance,
                        Status = customer.Wallet.Status
                    }
                }).FirstOrDefaultAsync() ?? null!;
        }

        public async Task<CustomerViewModel> CreateCustomer(CustomerCreateModel model)
        {
            var result = 0;
            var id = Guid.NewGuid();
            using (var transaction = _unitOfWork.Transaction())
            {
                try
                {
                    var accountId = await CreateAccount(model.Username, model.Password);
                    var walletId = await CreateWallet();

                    var customer = new Customer
                    {
                        Id = id,
                        Address = model.Address,
                        Gender = model.Gender,
                        Name = model.Name,
                        Phone = model.Phone,
                        AccountId = accountId,
                        WalletId = walletId,
                    };
                    _customerRepository.Add(customer);
                    result = await _unitOfWork.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            };
            return result > 0 ? await GetCustomer(id) : null!;
        }

        public async Task<CustomerViewModel> UpdateCustomer(Guid id, CustomerUpdateModel model)
        {
            var customer = await _customerRepository.GetMany(customer => customer.Id.Equals(id)).FirstOrDefaultAsync();
            if(customer != null)
            {
                if(model.Name != null) customer.Name = model.Name;
                if(model.Address != null) customer.Address = model.Address;
                if (model.Gender != null) customer.Gender = model.Gender;
                if (model.BankName != null) customer.BankName = model.BankName;
                if (model.BankAccountNumber != null) customer.BankAccountNumber = model.BankAccountNumber;
                if (model.Phone != null) customer.Phone = model.Phone;
                _customerRepository.Update(customer);
                var result = await _unitOfWork.SaveChanges();
                return await GetCustomer(id);
            }
            return null!;
        }

        // PRIVATE METHODS

        private async Task<Guid> CreateWallet()
        {
            var id = Guid.NewGuid();
            var wallet = new Wallet
            {
                Id = id,
                Balance = 0,
                Status = WalletStatus.Active.ToString(),
            };
            _walletRepository.Add(wallet);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? id : Guid.Empty;
        }

        private async Task<Guid> CreateAccount(string username, string password)
        {
            var id = Guid.NewGuid();
            var account = new Account
            {
                Id = id,
                Username = username,
                Password = password,
                Status = true
            };
            _accountRepository.Add(account);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? id : Guid.Empty;
        }
    }
}
