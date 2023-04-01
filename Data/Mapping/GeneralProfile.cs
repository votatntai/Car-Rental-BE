using AutoMapper;
using Data.Entities;
using Data.Models.Views;
using Type = Data.Entities.Type;

namespace Data.Mapping
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Car, CarViewModel>();

            CreateMap<Type, TypeViewModel>();

            CreateMap<Model, CarModelViewModel>();

            CreateMap<CarType, CarTypeViewModel>();

            CreateMap<Image, ImageViewModel>();

            CreateMap<FeedBack, FeedBackViewModel>();

            CreateMap<Car, CarViewModel>()
                .ForMember(carVM => carVM.ProductionCompany, config => config.MapFrom(car => car.Model.ProductionCompany));

            CreateMap<CarRegistration, CarRegistrationViewModel>()
                .ForMember(carRegistrationVM => carRegistrationVM.Calendars, config => config.MapFrom(carRegistration => carRegistration.CarRegistrationCalendars));
            
            CreateMap<Calendar, CalendarViewModel>();

            CreateMap<CarRegistrationCalendar, CarRegistrationCalendarViewModel>();

            CreateMap<CarCalendar, CarCalendarViewModel>();

            CreateMap<CarFeature, CarFeatureViewModel>();

            CreateMap<AdditionalCharge, AdditionalChargeViewModel>();

            CreateMap<DriverCalendar, DriverCalendarViewModel>();

            CreateMap<Showroom, ShowroomViewModel>();

            CreateMap<ProductionCompany, ProductionCompanyViewModel>();

            CreateMap<User, UserViewModel>()
                .ForMember(userVM => userVM.Status, config => config.MapFrom(user => user.Account.Status))
                .ForMember(userVM => userVM.Id, config => config.MapFrom(user => user.AccountId));

            CreateMap<Customer, CustomerViewModel>()
                .ForMember(customerVM => customerVM.Status, config => config.MapFrom(customer => customer.Account.Status))
                .ForMember(customerVM => customerVM.Id, config => config.MapFrom(customer => customer.AccountId));

            CreateMap<Driver, DriverViewModel>()
                .ForMember(driverVM => driverVM.AccountStatus, config => config.MapFrom(driver => driver.Account.Status))
                .ForMember(driverVM => driverVM.Id, config => config.MapFrom(driver => driver.AccountId));

            CreateMap<CarOwner, CarOwnerViewModel>()
                .ForMember(carOwnerVM => carOwnerVM.Status, config => config.MapFrom(carOwner => carOwner.Account.Status))
                .ForMember(carOwnerVM => carOwnerVM.Id, config => config.MapFrom(carOwner => carOwner.AccountId));

            CreateMap<Wallet, WalletViewModel>();

            CreateMap<Transaction, TransactionViewModel>();

            CreateMap<Notification, NotificationViewModel>();

            CreateMap<Order, OrderViewModel>();

            CreateMap<OrderDetail, OrderDetailViewModel>();

            CreateMap<Location, LocationViewModel>();

            CreateMap<Driver, DriverViewModel>();

            CreateMap<Promotion, PromotionViewModel>();

            CreateMap<Feature, FeatureViewModel>();

            CreateMap<CarFeature, CarFeatureViewModel>();

            CreateMap<FeedBack, FeedBackViewModel>();
        }
    }
}
