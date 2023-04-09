﻿using AutoMapper;
using Data.Entities;
using Data.Models.Views;
using Utility.Enums;
using Type = Data.Entities.Type;
using CarType = Data.Entities.CarType;

namespace Data.Mapping
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Type, TypeViewModel>();

            CreateMap<Model, CarModelViewModel>();

            CreateMap<CarType, CarTypeViewModel>();

            CreateMap<Image, ImageViewModel>();

            CreateMap<FeedBack, FeedBackViewModel>();

            CreateMap<Car, CarViewModel>()
                .ForMember(carVM => carVM.ProductionCompany, config => config.MapFrom(car => car.Model.ProductionCompany))
                .ForMember(carVM => carVM.Licenses,
                config => config.MapFrom(car => car.Images.Where(image => image.Type.Equals(ImageType.License.ToString()))))
                .ForMember(carVM => carVM.Thumbnails,
                config => config.MapFrom(car => car.Images.Where(image => image.Type.Equals(ImageType.Thumbnail.ToString()))));

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
                .ForMember(customerVM => customerVM.Id, config => config.MapFrom(customer => customer.AccountId))
                .ForMember(customerVM => customerVM.LicenseImages,
                config => config.MapFrom(customer => customer.Images))
                .ForMember(dest => dest.LicenseImages, opt => opt.Ignore()); ;

            CreateMap<Driver, DriverViewModel>()
                .ForMember(driverVM => driverVM.Id, config => config.MapFrom(driver => driver.Account.Id))
                .ForMember(driverVM => driverVM.AccountStatus, config => config.MapFrom(driver => driver.Account.Status));

            CreateMap<CarOwner, CarOwnerViewModel>()
                .ForMember(carOwnerVM => carOwnerVM.Status, config => config.MapFrom(carOwner => carOwner.Account.Status))
                .ForMember(carOwnerVM => carOwnerVM.Id, config => config.MapFrom(carOwner => carOwner.AccountId));

            CreateMap<Wallet, WalletViewModel>();

            CreateMap<Transaction, TransactionViewModel>();

            CreateMap<Notification, NotificationViewModel>()
                .ForMember(notificationVM => notificationVM.Data, config => config.MapFrom(notification => new NotificationDataViewModel
                {
                    CreateAt = notification.CreateAt,
                    IsRead = notification.IsRead,
                    Link = notification.Link,
                    Type = notification.Type
                }));

            CreateMap<Order, OrderViewModel>();

            CreateMap<OrderDetail, OrderDetailViewModel>();

            CreateMap<Location, LocationViewModel>();

            CreateMap<Promotion, PromotionViewModel>();

            CreateMap<Feature, FeatureViewModel>();

            CreateMap<CarFeature, CarFeatureViewModel>();

            CreateMap<FeedBack, FeedBackViewModel>();
        }
    }
}
