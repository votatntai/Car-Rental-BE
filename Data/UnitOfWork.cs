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
        private ICarRepository _car = null!;
        private ILocationRepository _location = null!;
        private IAdditionalChargeRepository _additionalCharge = null!;
        private ICarRegistrationRepository _carRegistration = null!;
        private ITransactionRepository _transaction = null!;
        private INotificationRepository _notification = null!;
        private IOrderRepository _order = null!;
        private IOrderDetailRepository _orderDetail = null!;
        private IProductionCompanyRepository _productionCompany = null!;
        private IModelRepository _model = null!;
        private IPromotionRepository _promotion = null!;
        private IFeedBackRepository _feedBack = null!;
        private IDeviceTokenRepository _deviceToken= null!;
        private ICalendarRepository _calendar= null!;
        private ICarCalendarRepository _carCalendar= null!;
        private IImageRepository _image = null!;
        private IShowroomRepository _showroom = null!;

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
        public ICarRepository Car
        {
            get { return _car ??= new CarRepository(_context); }
        }

        public ILocationRepository Location
        {
            get { return _location ??= new LocationRepository(_context); }
        }

        public IAdditionalChargeRepository AdditionalCharge
        {
            get { return _additionalCharge ??= new AdditionalChargeRepository(_context); }
        }

        public ICarRegistrationRepository CarRegistration
        {
            get { return _carRegistration ??= new CarRegistrationRepository(_context); }
        }

        public ITransactionRepository Transactions
        {
            get { return _transaction ??= new TransactionRepository(_context); }
        }

        public INotificationRepository Notification
        {
            get { return _notification ??= new NotificationRepository(_context); }
        }

        public IOrderRepository Order
        {
            get { return _order ??= new OrderRepository(_context); }
        }

        public IOrderDetailRepository OrderDetail
        {
            get { return _orderDetail ??= new OrderDetailRepository(_context); }
        }

        public IProductionCompanyRepository ProductionCompany
        {
            get { return _productionCompany ??= new ProductionCompanyRepository(_context); }
        }

        public IModelRepository Model
        {
            get { return _model ??= new ModelRepository(_context); }
        }

        public IPromotionRepository Promotion
        {
            get { return _promotion ??= new PromotionRepository(_context); }
        }

        public IFeedBackRepository FeedBack
        {
            get { return _feedBack ??= new FeedBackRepository(_context); }
        }

        public IDeviceTokenRepository DeviceToken
        {
            get { return _deviceToken ??= new DeviceTokenRepository (_context); }
        }

        public ICalendarRepository Calendar
        {
            get { return _calendar ??= new CalendarRepository(_context); }
        }
        public ICarCalendarRepository CarCalendar
        {
            get { return _carCalendar ??= new CarCalendarRepository(_context); }
        }

        public IImageRepository Image
        {
            get { return _image ??= new ImageRepository(_context); }
        }

        public IShowroomRepository Showroom
        {
            get { return _showroom ??= new ShowroomRepository(_context); }
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
