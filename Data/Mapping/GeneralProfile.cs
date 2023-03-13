using AutoMapper;
using Data.Entities;
using Data.Models.Views;

namespace Data.Mapping
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Car, CarViewModel>();
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
        }
    }
}
