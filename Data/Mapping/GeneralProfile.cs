using AutoMapper;
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

            //var emptyList = new List<FeedBackViewModel>();
            //var img = new List<ImageViewModel>
            //{
            //    new ImageViewModel
            //    {
            //        Id = Guid.NewGuid(),
            //        Type = ImageType.Thumbnail.ToString(),
            //        Url = "https://storage.googleapis.com/car-rental-236aa.appspot.com/attachments/7f3c09aa-603a-4f91-b228-88fed5866b42?X-Goog-Algorithm=GOOG4-RSA-SHA256&X-Goog-Credential=firebase-adminsdk-ym9hq%40car-rental-236aa.iam.gserviceaccount.com%2F20230410%2Fauto%2Fstorage%2Fgoog4_request&X-Goog-Date=20230410T031936Z&X-Goog-Expires=86400&X-Goog-SignedHeaders=host&X-Goog-Signature=0c0458dc4d656665fbba018f6c61f2aa979e0812b05a0598e1faf563ad36b46bdf9a84299c6c79cd82eadc364f9d2a9e2f139538fd593f6292662ab97f13629e73fa90b283ccdc650f3cfbcc6cc5b952101cae49c6b17f76bc0dec63feac87edf35fb01f02c52d9cb4985e74c30deba13e04162cbb7cd7b4b47f024ecb229fc89505e4d19e31fe0b85c91343f20a74127138fc3eda41862899b2683fc4f5fff9084a2012ca7fa962485d9712931f09ebd8acecf3c7ebdb3df01bb3fa1ec259d99963cea46e92e60ba3d5ee304d73f97443b713a6657b5e2941870c67b93e358f6853b72a7905679cf4eb4d0d6fcd1467448d7d50d8698142f7953d70a931fda9"
            //    }
            //};
            //var emptyList1 = new List<CarFeatureViewModel>();
            //var emptyList2 = new List<CarTypeViewModel>();
            //var emptyList3 = new List<DriverCalendarViewModel>();

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
