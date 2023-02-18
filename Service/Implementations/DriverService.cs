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
    public class DriverService : BaseService, IDriverService
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IWalletRepository _walletRepository;

        public DriverService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _driverRepository = unitOfWork.Driver;
            _accountRepository = unitOfWork.Account;
            _walletRepository = unitOfWork.Wallet;
        }

        public async Task<DriverViewModel> GetDriver(Guid id)
        {
            return await _driverRepository.GetMany(driver => driver.Id.Equals(id)).Select(driver => new DriverViewModel
            {
                Id = driver.Id,
                Address = driver.Address,
                AvartarUrl = driver.AvartarUrl,
                BankAccountNumber = driver.BankAccountNumber,
                BankName = driver.BankName,
                Gender = driver.Gender,
                Name = driver.Name,
                Phone = driver.Phone,
                Wallet = new WalletViewModel
                {
                    Id = driver.Wallet.Id,
                    Balance = driver.Wallet.Balance,
                    Status = driver.Wallet.Status,
                },
                Location = driver.Location != null ? new LocationViewModel
                {
                    Id = driver.Location.Id,
                    Latitude = driver.Location.Latitude,
                    Longitude = driver.Location.Longitude,
                } : null!,
                Star = driver.Star,
                Status = driver.Status,
            }).FirstOrDefaultAsync() ?? null!;
        }

        public async Task<DriverViewModel> CreateDriver(DriverCreateModel model)
        {
            var accountId = await CreateAccount(model.Username, model.Password);
            var walletId = await CreateWallet();
            var id = Guid.NewGuid();
            var driver = new Driver
            {
                Id = id,
                Address = model.Address,
                Gender = model.Gender,
                Name = model.Name,
                Phone = model.Phone,
                AccountId = accountId,
                WalletId = walletId,
                Status = DriverStatus.Idle.ToString(),
            };
            _driverRepository.Add(driver);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetDriver(id) : null!;
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
