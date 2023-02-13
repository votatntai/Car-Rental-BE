using Data.Entities;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;

namespace Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CarRentalContext _context;

        private IAccountRepository _account = null!;
        private IUserRepository _user = null!;


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

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
