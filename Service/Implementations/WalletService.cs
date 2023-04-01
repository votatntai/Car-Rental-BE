using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Data.Models.Views;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Service.Implementations
{
    public class WalletService : BaseService, IWalletService
    {
        private new readonly IMapper _mapper;
        private readonly IWalletRepository _walletRepository;
        public WalletService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _mapper = mapper;
            _walletRepository = unitOfWork.Wallet;
        }

        public async Task<WalletViewModel> GetWallet(Guid userId)
        {
            return await _walletRepository.GetMany(wallet =>
                wallet.Customer != null ? wallet.Customer.AccountId.Equals(userId) :
                wallet.CarOwner != null ? wallet.CarOwner.AccountId.Equals(userId) :
                wallet.Driver != null ? wallet.Driver.AccountId.Equals(userId) :
                wallet.User != null ? wallet.User.AccountId.Equals(userId) :
                false).ProjectTo<WalletViewModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync() ?? null!;
        }
    }
}
