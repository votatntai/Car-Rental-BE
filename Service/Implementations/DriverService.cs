using Data;
using Data.Entities;
using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
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

        public async Task<ListViewModel<DriverViewModel>> GetDrivers(DriverFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _driverRepository.GetMany(driver => filter.Name != null ? driver.Name.Contains(filter.Name) : true)
                .Select(driver => new DriverViewModel
                {
                    Id = driver.Id,
                    Gender = driver.Gender,
                    Name = driver.Name,
                    AvartarUrl = driver.AvartarUrl,
                    Phone = driver.Phone,
                    Wallet = new WalletViewModel
                    {
                        Id = driver.Wallet.Id,
                        Balance = driver.Wallet.Balance,
                        Status = driver.Wallet.Status
                    },
                    Address= driver.Address,
                    BankAccountNumber= driver.BankAccountNumber,
                    BankName= driver.BankName,
                    Star = driver.Star,
                    Location = driver.Location != null ? new LocationViewModel
                    {
                        Id = driver.Location.Id,
                        Latitude= driver.Location.Latitude,
                        Longitude= driver.Location.Longitude,
                    }: null!,
                    Status= driver.Status,
                });
            var drivers = await query.Skip(pagination.PageNumber * pagination.PageSize).Take(pagination.PageSize).AsNoTracking().ToListAsync();
            var totalRow = await query.AsNoTracking().CountAsync();
            if (drivers != null || drivers != null && drivers.Any())
            {
                return new ListViewModel<DriverViewModel>
                {
                    Pagination = new PaginationViewModel
                    {
                        PageNumber = pagination.PageNumber,
                        PageSize = pagination.PageSize,
                        TotalRow = totalRow
                    },
                    Data = drivers
                };
            }
            return null!;
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
            var result = 0;
            var id = Guid.NewGuid();
            using (var transaction = _unitOfWork.Transaction())
            {
                try
                {
                    var accountId = await CreateAccount(model.Username, model.Password);
                    var walletId = await CreateWallet();

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
                    result = await _unitOfWork.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            };
            return result > 0 ? await GetDriver(id) : null!;
        }

        public async Task<DriverViewModel> UpdateDriver(Guid id, DriverUpdateModel model)
        {
            var driver = await _driverRepository.GetMany(driver => driver.Id.Equals(id))
                .Include(driver => driver.Account)
                .FirstOrDefaultAsync();
            if (driver != null)
            {
                if (model.Name != null) driver.Name = model.Name;
                if (model.Address != null) driver.Address = model.Address;
                if (model.Gender != null) driver.Gender = model.Gender;
                if (model.BankName != null) driver.BankName = model.BankName;
                if (model.BankAccountNumber != null) driver.BankAccountNumber = model.BankAccountNumber;
                if (model.Phone != null) driver.Phone = model.Phone;
                if (model.Password != null) driver.Account.Password = model.Password;
                _driverRepository.Update(driver);
                var result = await _unitOfWork.SaveChanges();
                return await GetDriver(id);
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
