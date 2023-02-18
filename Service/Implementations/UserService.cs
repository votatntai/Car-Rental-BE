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
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IWalletRepository _walletRepository;

        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _userRepository = unitOfWork.User;
            _accountRepository = unitOfWork.Account;
            _walletRepository = unitOfWork.Wallet;
        }

        // PUBLIC METHODS

        public async Task<UserViewModel> GetUser(Guid id)
        {
            return await _userRepository.GetMany(user => user.Id.Equals(id)).Select(user => new UserViewModel
            {
                Id = user.Id,
                Gender = user.Gender,
                 Name =user.Name,
                 AvartarUrl = user.AvartarUrl,
                 Phone = user.Phone,
                 Role = user.Role,
                 Wallet = new WalletViewModel
                 {
                     Id = user.Wallet.Id,
                     Balance = user.Wallet.Balance,
                     Status = user.Wallet.Status
                 }
            }).FirstOrDefaultAsync() ?? null!;
        }

        public async Task<UserViewModel> CreateManager(UserCreateModel model)
        {
            var accountId = await CreateAccount(model.Username, model.Password);
            var walletId = await CreateWallet();
            var id = Guid.NewGuid();
            var user = new User
            {
                Id = id,
                Gender = model.Gender,
                Name = model.Name,
                Phone = model.Phone,
                Role = UserRole.Manager.ToString(),
                AccountId = accountId,
                WalletId = walletId,
            };
            _userRepository.Add(user);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetUser(id) : null!;
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
