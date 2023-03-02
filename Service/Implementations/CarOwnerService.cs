using Data;
using Data.Entities;
using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;
using Data.Repositories.Implementations;
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

        // PUBLIC METHOD

        public async Task<ListViewModel<CarOwnerViewModel>> GetCarOwners(CarOwnerFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _carOwnerRepository.GetMany(carOwner => filter.Name != null ? carOwner.Name.Contains(filter.Name) : true)
                .Select(carOwner => new CarOwnerViewModel
                {
                    Id = carOwner.Id,
                    Gender = carOwner.Gender,
                    Name = carOwner.Name,
                    AvatarUrl = carOwner.AvatarUrl,
                    Phone = carOwner.Phone,
                    Wallet = new WalletViewModel
                    {
                        Id = carOwner.Wallet.Id,
                        Balance = carOwner.Wallet.Balance,
                        Status = carOwner.Wallet.Status
                    },
                    Address = carOwner.Address,
                    BankAccountNumber = carOwner.BankAccountNumber,
                    BankName = carOwner.BankName
                });
            var carOwners = await query.Skip(pagination.PageNumber * pagination.PageSize).Take(pagination.PageSize).AsNoTracking().ToListAsync();
            var totalRow = await query.AsNoTracking().CountAsync();
            if (carOwners != null || carOwners != null && carOwners.Any())
            {
                return new ListViewModel<CarOwnerViewModel>
                {
                    Pagination = new PaginationViewModel
                    {
                        PageNumber = pagination.PageNumber,
                        PageSize = pagination.PageSize,
                        TotalRow = totalRow
                    },
                    Data = carOwners
                };
            }
            return null!;
        }

        public async Task<CarOwnerViewModel> GetCarOwner(Guid id)
        {
            return await _carOwnerRepository.GetMany(carOwner => carOwner.Id.Equals(id)).Select(carOwner => new CarOwnerViewModel
            {
                Id = carOwner.Id,
                Address = carOwner.Address,
                AvatarUrl = carOwner.AvatarUrl,
                BankAccountNumber = carOwner.BankAccountNumber,
                BankName = carOwner.BankName,
                Gender = carOwner.Gender,
                Name = carOwner.Name,
                Phone = carOwner.Phone,
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

        public async Task<CarOwnerViewModel> UpdateCarOwner(Guid id, CarOwnerUpdateModel model)
        {
            var carOwner = await _carOwnerRepository.GetMany(carOwner => carOwner.Id.Equals(id))
                .Include(carOwner => carOwner.Account)
                .FirstOrDefaultAsync();
            if (carOwner != null)
            {
                if (model.Name != null) carOwner.Name = model.Name;
                if (model.Address != null) carOwner.Address = model.Address;
                if (model.Gender != null) carOwner.Gender = model.Gender;
                if (model.BankName != null) carOwner.BankName = model.BankName;
                if (model.BankAccountNumber != null) carOwner.BankAccountNumber = model.BankAccountNumber;
                if (model.Phone != null) carOwner.Phone = model.Phone;
                if (model.Password != null) carOwner.Account.Password = model.Password;
                _carOwnerRepository.Update(carOwner);
                var result = await _unitOfWork.SaveChanges();
                return await GetCarOwner(id);
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
