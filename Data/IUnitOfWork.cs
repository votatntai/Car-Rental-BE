
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

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
        public ICarRepository Car { get; }
        public ILocationRepository Location { get; }
        public IAdditionalChargeRepository AdditionalCharge { get; }
        public ICarRegistrationRepository CarRegistration { get; }
        public ITransactionRepository Transactions { get; }
        public INotificationRepository Notification { get; }
        public IOrderRepository Order { get; }
        public IOrderDetailRepository OrderDetail { get; }
        public IProductionCompanyRepository ProductionCompany { get; }
        public IModelRepository Model { get; }
        public IPromotionRepository Promotion { get; }
        public IFeedBackRepository FeedBack { get; }
        public IDeviceTokenRepository DeviceToken { get; }
        public ICalendarRepository Calendar { get; }
        public ICarCalendarRepository CarCalendar { get; }
        public IImageRepository Image { get; }
        public IShowroomRepository Showroom { get; }

        Task<int> SaveChanges();
        IDbContextTransaction Transaction();
    }
}
