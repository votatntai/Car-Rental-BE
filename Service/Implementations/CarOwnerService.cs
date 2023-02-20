using Data;
using Data.Entities;
using Data.Models.Create;
using Data.Models.Views;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using Utility.Constants;

namespace Service.Implementations
{
    public class CarOwnerService : BaseService, ICarOwnerService
    {
        private readonly ICarOwnerRepository _carOwnerRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IWalletRepository _walletRepository;

        public CarOwnerService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _carOwnerRepository = unitOfWork.CarOwner;
            _accountRepository = unitOfWork.Account;
            _walletRepository = unitOfWork.Wallet;
        }

        public async Task<CarOwnerViewModel> GetCarOwner(Guid id)
        {
            return await _carOwnerRepository.GetMany(carOwner => carOwner.Id.Equals(id)).Select(carOwner => new CarOwnerViewModel
            {
                Id = carOwner.Id,
                Address= carOwner.Address,
                AvartarUrl= carOwner.AvartarUrl,
                BankAccountNumber= carOwner.BankAccountNumber,
                BankName= carOwner.BankName,
                Gender= carOwner.Gender,
                Name= carOwner.Name,
                Phone= carOwner.Phone,
                Wallet = new WalletViewModel
                {
                    Id = carOwner.Wallet.Id,
                    Balance = carOwner.Wallet.Balance,
                    Status = carOwner.Wallet.Status,
                }
            }).FirstOrDefaultAsync() ?? null!;
        }

        public async Task<CarOwnerViewModel> CreateCarOwner(CarOwnerCreateModel model)
        {
            var result = 0;
            var id = Guid.NewGuid();
            using (var transaction = _unitOfWork.Transaction())
            {
                try
                {
                    var accountId = await CreateAccount(model.Username, model.Password);
                    var walletId = await CreateWallet();

                    var carOwner = new CarOwner
                    {
                        Id = id,
                        Address = model.Address,
                        Gender = model.Gender,
                        Name = model.Name,
                        Phone = model.Phone,
                        AccountId = accountId,
                        WalletId = walletId,
                    };
                    _carOwnerRepository.Add(carOwner);
                    result = await _unitOfWork.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            };
            return result > 0 ? await GetCarOwner(id) : null!;
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
