using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Data.Entities;
using Data.Models.Create;
using Data.Models.Get;
using Data.Models.Update;
using Data.Models.Views;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Service.Interfaces;
using Utility.Enums;

namespace Service.Implementations
{
    public class CarOwnerService : BaseService, ICarOwnerService
    {
        private readonly ICarOwnerRepository _carOwnerRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IWalletRepository _walletRepository;
        private new readonly IMapper _mapper;

        public CarOwnerService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _carOwnerRepository = unitOfWork.CarOwner;
            _accountRepository = unitOfWork.Account;
            _walletRepository = unitOfWork.Wallet;
            _mapper = mapper;
        }

        // PUBLIC METHOD

        public async Task<ListViewModel<CarOwnerViewModel>> GetCarOwners(CarOwnerFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _carOwnerRepository.GetMany(carOwner => filter.Name != null ? carOwner.Name.Contains(filter.Name) : true)
                .Include(carOwner => carOwner.Account)
                .ProjectTo<CarOwnerViewModel>(_mapper.ConfigurationProvider);
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
            return await _carOwnerRepository.GetMany(carOwner => carOwner.AccountId.Equals(id))
                .Include(carOwner => carOwner.Account)
                .ProjectTo<CarOwnerViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? null!;
        }

        public async Task<CarOwnerViewModel> CreateCarOwner(CarOwnerCreateModel model)
        {
            var result = 0;
            var accountId = Guid.Empty;
            using (var transaction = _unitOfWork.Transaction())
            {
                try
                {
                    accountId = await CreateAccount(model.Username, model.Password);
                    var walletId = await CreateWallet();

                    var carOwner = new CarOwner
                    {
                        AccountId = accountId,
                        Address = model.Address,
                        Gender = model.Gender,
                        Name = model.Name,
                        Phone = model.Phone,
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
                return result > 0 ? await GetCarOwner(accountId) : null!;
        }

        public async Task<CarOwnerViewModel> UpdateCarOwner(Guid id, CarOwnerUpdateModel model)
        {
            var carOwner = await _carOwnerRepository.GetMany(carOwner => carOwner.AccountId.Equals(id))
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
                if (model.Status != null) carOwner.Account.Status = (bool)model.Status;
                if (model.IsAutoAcceptOrder != null) carOwner.IsAutoAcceptOrder = (bool)model.IsAutoAcceptOrder;
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
