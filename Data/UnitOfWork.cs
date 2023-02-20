using Data.Entities;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CarRentalContext _context;

        private IAccountRepository _account = null!;
        private IUserRepository _user = null!;
        private IWalletRepository _wallet = null!;
        private ICarOwnerRepository _carOwner = null!;
        private IDriverRepository _driver = null!;
        private ICustomerRepository _customer = null!;

        public UnitOfWork(CarRentalContext context)
        {
            _context = context;
        }

        public IAccountRepository Account
        {
            get { return _account ??= new AccountRepository(_context); }
        }
        public IUserRepository User
        {
            get { return _user ??= new UserRepository(_context); }
        }
        public IWalletRepository Wallet
        {
            get { return _wallet ??= new WalletRepository(_context); }
        }

        public ICarOwnerRepository CarOwner
        {
            get { return _carOwner ??= new CarOwnerRepository(_context); }
        }
        public IDriverRepository Driver
        {
            get { return _driver ??= new DriverRepository(_context); }
        }
        public ICustomerRepository Customer
        {
            get { return _customer ??= new CustomerRepository(_context); }
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }

        public IDbContextTransaction Transaction()
        {
            return _context.Database.BeginTransaction();
        }
    }
}
