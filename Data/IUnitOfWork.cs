
using Data.Repositories.Interfaces;

namespace Data
{
    public interface IUnitOfWork
    {
        public IAccountRepository Account { get; }
        public IUserRepository User { get; }


        Task<int> SaveChanges();
    }
}
