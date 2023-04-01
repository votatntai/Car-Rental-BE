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
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IWalletRepository _walletRepository;
        private new readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _userRepository = unitOfWork.User;
            _accountRepository = unitOfWork.Account;
            _walletRepository = unitOfWork.Wallet;
            _mapper = mapper;
        }

        // PUBLIC METHODS

        public async Task<ListViewModel<UserViewModel>> GetUsers(UserFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _userRepository.GetMany(user => (filter.Name != null ? user.Name.Contains(filter.Name) : true)
            && user.Role.Equals(UserRole.Manager.ToString()))
                .Include(user => user.Account)
                .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider);
        var order = filter.Order != null && filter.Order.Equals(OrderBy.Asc.ToString().ToLower()) ? query.OrderBy(user => user.Name) : query.OrderByDescending(user => user.Name);
        var users = await order.Skip(pagination.PageNumber * pagination.PageSize).Take(pagination.PageSize).AsNoTracking().ToListAsync();
        var totalRow = await query.AsNoTracking().CountAsync();
            if (users != null || users != null && users.Any())
            {
                return new ListViewModel<UserViewModel>
                {
                    Pagination = new PaginationViewModel
                    {
                        PageNumber = pagination.PageNumber,
                        PageSize = pagination.PageSize,
                        TotalRow = totalRow
    },
                    Data = users
};
            }
            return null!;
        }

        public async Task<UserViewModel> GetUser(Guid id)
{
    return await _userRepository.GetMany(user => user.AccountId.Equals(id))
        .Include(user => user.Account)
        .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync() ?? null!;
}

public async Task<UserViewModel> CreateManager(UserCreateModel model)
{
    var result = 0;
    var accountId = Guid.Empty;
    using (var transaction = _unitOfWork.Transaction())
    {
        try
        {
            accountId = await CreateAccount(model.Username, model.Password);
            var walletId = await CreateWallet();

            var user = new User
            {
                Gender = model.Gender,
                Name = model.Name,
                Phone = model.Phone,
                AccountId = accountId,
                WalletId = walletId,
                Role = UserRole.Manager.ToString()
            };
            _userRepository.Add(user);
            result = await _unitOfWork.SaveChanges();
            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    };
    return result > 0 ? await GetUser(accountId) : null!;
}

public async Task<UserViewModel> UpdateUser(Guid id, UserUpdateModel model)
        {
            var user = await _userRepository.GetMany(user => user.AccountId.Equals(id)).Include(user => user.Account).FirstOrDefaultAsync();
            if (user != null)
            {
                if (model.Name != null) user.Name = model.Name;
                if (model.Gender != null) user.Gender = model.Gender;
                if (model.Phone != null) user.Phone = model.Phone;
                if (model.Password != null) user.Account.Password = model.Password;
                if (model.Status != null) user.Account.Status = (bool)model.Status;
                _userRepository.Update(user);
                var result = await _unitOfWork.SaveChanges();
                return result > 0 ? await GetUser(id) : null!;
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
