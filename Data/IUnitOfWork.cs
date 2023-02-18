
using Data.Repositories.Interfaces;

namespace Data
{
    public interface IUnitOfWork
    {
        public IAccountRepository Account { get; }
        public IUserRepository User { get; }
        public IWalletRepository Wallet { get; }
        public ICarOwnerRepository CarOwner { get; }
        public IDriverRepository Driver { get; }
        public ICustomerRepository Customer { get; }


        Task<int> SaveChanges();
    }
}
