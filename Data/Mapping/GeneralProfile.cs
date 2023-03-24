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

            CreateMap<Car, CarViewModel>();

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
                .ForMember(userVM => userVM.Status, config => config.MapFrom(user => user.Account.Status));

            CreateMap<Customer, CustomerViewModel>()
                .ForMember(customerVM => customerVM.Status, config => config.MapFrom(customer => customer.Account.Status));

            CreateMap<Driver, DriverViewModel>()
                .ForMember(DriverVM => DriverVM.AccountStatus, config => config.MapFrom(Driver => Driver.Account.Status));

            CreateMap<CarOwner, CarOwnerViewModel>()
                .ForMember(carOwnerVM => carOwnerVM.Status, config => config.MapFrom(carOwner => carOwner.Account.Status));

            CreateMap<Wallet, WalletViewModel>();

            CreateMap<Transaction, TransactionViewModel>();

            CreateMap<Notification, NotificationViewModel>();

            CreateMap<Order, OrderViewModel>();

            CreateMap<OrderDetail, OrderDetailViewModel>();

            CreateMap<Location, LocationViewModel>();

            CreateMap<Driver, DriverViewModel>();

            CreateMap<Promotion, PromotionViewModel>();
        }
    }
}
