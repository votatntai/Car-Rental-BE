using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Data.Entities;
using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using Utility.Enums;

namespace Service.Implementations
{
    public class DriverService : BaseService, IDriverService
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly ILocationRepository _locationRepository;
        private new readonly IMapper _mapper;

        public DriverService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _driverRepository = unitOfWork.Driver;
            _accountRepository = unitOfWork.Account;
            _walletRepository = unitOfWork.Wallet;
            _locationRepository = unitOfWork.Location;
            _mapper = mapper;
        }

        public async Task<ListViewModel<DriverViewModel>> GetDrivers(DriverFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _driverRepository.GetMany(driver => filter.Name != null ? driver.Name.Contains(filter.Name) : true)
                .ProjectTo<DriverViewModel>(_mapper.ConfigurationProvider);
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
            var driver = await _driverRepository.GetMany(driver => driver.AccountId.Equals(id)).FirstOrDefaultAsync();
            return await _driverRepository.GetMany(driver => driver.AccountId.Equals(id))
                .ProjectTo<DriverViewModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync() ?? null!;
        }

        public async Task<DriverViewModel> CreateDriver(DriverCreateModel model)
        {
            var result = 0;
            var accountId = Guid.Empty;
            using (var transaction = _unitOfWork.Transaction())
            {
                try
                {
                    accountId = await CreateAccount(model.Username, model.Password);
                    var locationId = await CreateLocation(model.Location);
                    var walletId = await CreateWallet();

                    var driver = new Driver
                    {
                        Address = model.Address,
                        Gender = model.Gender,
                        Name = model.Name,
                        BankName = model.BankName,
                        BankAccountNumber = model.BankAccountNumber,
                        Phone = model.Phone,
                        AccountId = accountId,
                        WalletId = walletId,
                        Status = DriverStatus.Idle.ToString(),
                        WishAreaId = locationId
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
            return result > 0 ? await GetDriver(accountId) : null!;
        }

        public async Task<DriverViewModel> UpdateDriver(Guid id, DriverUpdateModel model)
        {
            var driver = await _driverRepository.GetMany(driver => driver.AccountId.Equals(id))
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
                if (model.Status != null) driver.Status = model.Status;
                if (model.AccountStatus != null) driver.Account.Status = (bool)model.AccountStatus;
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

        private async Task<Guid> CreateLocation(LocationCreateModel model)
        {
            var id = Guid.NewGuid();
            var location = new Location
            {
                Id = id,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
            };
            _locationRepository.Add(location);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? id : Guid.Empty;
        }
    }
}
