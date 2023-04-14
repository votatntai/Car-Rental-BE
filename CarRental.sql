Create Database CarRental
Go
Use CarRental
Go
--Lưu trữ tài khoản của cả Manager, Admin, CarOwner, Customer
Create Table Account(
	Id uniqueidentifier primary key,
	Username varchar(256) not null unique,
	Password varchar(256) not null,
	Status bit not null
)
Go
--Ví của người dùng
Create Table Wallet(
	Id uniqueidentifier primary key,
	Balance float not null default 0,
	Status nvarchar(256) not null
)
Go
--Thông tin vị trí
Create Table [Location](
	Id uniqueidentifier primary key,
	Longitude float not null,
	Latitude float not null
)
Go
--Thông tin khách hàng
Create Table Customer(
	AccountId uniqueidentifier primary key references Account(Id) not null,
	Name nvarchar(256) not null,
	Address nvarchar(256),
	Phone varchar(256) not null unique,
	Gender nvarchar(256) not null,
	AvatarUrl nvarchar(max),
	BankAccountNumber nvarchar(256),
	BankName nvarchar(256),
	IsLicenseValid bit not null default 0,
	WalletId uniqueidentifier foreign key references Wallet(Id) not null unique,
)
Go
--Thông tin chủ xe
Create Table CarOwner(
	AccountId uniqueidentifier primary key references Account(Id) not null,
	Name nvarchar(256) not null,
	Address nvarchar(256),
	Phone varchar(256) not null unique,
	Gender nvarchar(256) not null,
	AvatarUrl nvarchar(max),
	BankAccountNumber nvarchar(256),
	BankName nvarchar(256),
	IsAutoAcceptOrder bit not null default 0,
	WalletId uniqueidentifier foreign key references Wallet(Id) not null unique,
)
Go
--Thông tin tài xế
Create Table Driver(
	AccountId uniqueidentifier primary key references Account(Id) not null,
	Name nvarchar(256) not null,
	Address nvarchar(256),
	Phone varchar(256) not null unique,
	Gender nvarchar(256) not null,
	AvatarUrl nvarchar(max),
	Star float,
	BankAccountNumber nvarchar(256),
	BankName nvarchar(256),
	WalletId uniqueidentifier foreign key references Wallet(Id) not null unique,
	LocationId uniqueidentifier foreign key references [Location](Id),
	WishAreaId uniqueidentifier foreign key references [Location](Id),
	MinimumTrip int default 5,
	Status nvarchar(256) not null
)
Go
--Thông tin nhân viên (Admin và Manager)
Create Table [User](
	AccountId uniqueidentifier primary key references Account(Id) not null,
	Name nvarchar(256) not null,
	Phone varchar(256) not null unique,
	Gender nvarchar(256) not null,
	AvatarUrl nvarchar(max),
	Role nvarchar(256) not null,
	WalletId uniqueidentifier foreign key references Wallet(Id) not null unique,
)
Go
--Lưu token của thiết bị để gửi thông báo
Create Table DeviceToken(
	Id uniqueidentifier primary key,
	AccountId uniqueidentifier foreign key references Account(Id) not null,
	Token nvarchar(max),
	CreateAt datetime not null default getdate()
)
Go
--Thông tin showroom
Create Table Showroom(
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	Description nvarchar(max),
	LocationId uniqueidentifier foreign key references [Location](Id),
)
Go
--Thông tin hãng xe (một hãng xe có nhiều model)
Create Table ProductionCompany(
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	Description nvarchar(max),
)
Go
--Thông tin model xe (một model xe thuộc về 1 hãng xe)
Create Table Model(
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	CellingPrice float not null,
	FloorPrice float not null,
	Seater int not null,
	Chassis nvarchar(256) not null,
	YearOfManufacture int not null,
	TransmissionType nvarchar(256) not null,
	FuelType nvarchar(256) not null,
	FuelConsumption nvarchar(256) not null,
	ProductionCompanyId uniqueidentifier foreign key references ProductionCompany(Id) not null,
)
Go
--Thông tin chi phí phát sinh của xe
Create Table AdditionalCharge(
	Id uniqueidentifier primary key,
	MaximumDistance int not null,
	DistanceSurcharge float not null,
	TimeSurcharge float not null,
	AdditionalDistance float not null,
	AdditionalTime float not null
)
Go
--Thông tin xe
Create Table Car(
	Id uniqueidentifier primary key,
	Name nvarchar(256),
	LicensePlate varchar(256) not null,
	Price float not null,
	CreateAt datetime not null default getdate(),
	Description nvarchar(max),
	ModelId uniqueidentifier foreign key references Model(Id) not null,
	LocationId uniqueidentifier foreign key references [Location](Id),
	AdditionalChargeId uniqueidentifier foreign key references AdditionalCharge(Id),
	DriverId uniqueidentifier foreign key references Driver(AccountId),
	CarOwnerId uniqueidentifier foreign key references CarOwner(AccountId) not null,
	ShowroomId uniqueidentifier foreign key references Showroom(Id),
	Rented int not null default 0,
	ReceiveStartTime time not null default '06:00:00',
	ReceiveEndTime time not null default '22:00:00',
	ReturnStartTime time not null default '06:00:00',
	ReturnEndTime time not null default '22:00:00',
	Star float,
	IsTracking bit not null default 0,
	Status nvarchar(256) not null,
)
Go
--Tính năng xe có
Create Table Feature(
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	Description nvarchar(max),
)
Go
--Mỗi xe có nhiều tính năng mỗi tính năng sẽ là 1 record trong bảng này
Create Table CarFeature(
	CarId uniqueidentifier foreign key references Car(Id),
	FeatureId uniqueidentifier foreign key references Feature(Id),
	Description nvarchar(max),
	Primary key (CarId, FeatureId)
)
Go
--Kiểu xe (xe bán tải, sedan, mini, 4 chỗ, 5 chỗ...)
Create Table [Type](
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	Description nvarchar(max),
)
Go
--Xe có nhiều kiểu (vừa sedan nhưng lại 4 chỗ) sẽ lưu trong này
Create Table CarType(
	CarId uniqueidentifier foreign key references Car(Id),
	TypeId uniqueidentifier foreign key references [Type](Id),
	Description nvarchar(max),
	Primary key (CarId, TypeId)
)
Go
--Phiếu đăng ký xe
Create Table CarRegistration(
	Id uniqueidentifier primary key,
	Name nvarchar(256),
	LicensePlate varchar(256) not null,
	TransmissionType nvarchar(256) not null,
	FuelType nvarchar(256) not null,
	Seater int not null,
	Price float not null,
	FuelConsumption nvarchar(256) not null,
	YearOfManufacture int not null,
	ProductionCompany nvarchar(256) not null,
	Model varchar(256) not null,
	Chassis varchar(256) not null,
	Location nvarchar(256) not null,
	CreateAt datetime not null default getdate(),
	CarOwnerId uniqueidentifier foreign key references CarOwner(AccountId) not null,
	AdditionalChargeId uniqueidentifier foreign key references AdditionalCharge(Id),
	Description nvarchar(max),
	Status bit not null default 0
)
Go
--Lịch trong tuần
Create Table Calendar(
	Id uniqueidentifier primary key,
	Weekday nvarchar(256) not null,
	StartTime time not null default '06:00:00',
	EndTime time not null default '22:00:00',
)
Go
--Lịch trong tuần của xe
Create Table CarCalendar(
	CalendarId uniqueidentifier foreign key references Calendar(Id) not null,
	CarId uniqueidentifier foreign key references Car(Id) not null,
	Description nvarchar(max),
	Primary key (CalendarId, CarId)
)
Go
--Lịch trong tuần của xe lúc tạo phiếu đăng ký
Create Table CarRegistrationCalendar(
	CalendarId uniqueidentifier foreign key references Calendar(Id) not null,
	CarRegistrationId uniqueidentifier foreign key references CarRegistration(Id) not null,
	Description nvarchar(max),
	Primary key (CalendarId, CarRegistrationId)
)
Go
--Lịch của tài xế (thường sẽ trùng với lịch của xe mà tài xế này lái)
Create Table DriverCalendar(
	CalendarId uniqueidentifier foreign key references Calendar(Id) not null,
	CarId uniqueidentifier foreign key references Car(Id) not null,
	DriverId uniqueidentifier foreign key references Driver(AccountId) not null,
	Description nvarchar(max),
	Primary key (CalendarId, DriverId, CarId)
)
Go
--Bảng thông báo lưu lại những thông báo đã được gửi đi
Create Table [Notification](
	Id uniqueidentifier primary key,
	Title nvarchar(256) not null,
	Body nvarchar(max) not null,
	Type nvarchar(256),
	Link nvarchar(256),
	AccountId uniqueidentifier foreign key references Account(Id) not null,
	IsRead bit not null default 0,
	CreateAt datetime not null default getdate(),
)
Go
--Bảng lưu lại những giao dịch trong ứng dụng
Create Table [Transaction](
	Id uniqueidentifier primary key,
	UserId uniqueidentifier foreign key references [User](AccountId),
	DriverId uniqueidentifier foreign key references Driver(AccountId),
	CustomerId uniqueidentifier foreign key references Customer(AccountId),
	CarOwnerId uniqueidentifier foreign key references CarOwner(AccountId),
	Type nvarchar(256) not null,
	Amount float not null,
	Description nvarchar(max),
	CreateAt datetime default getDate(),
	Status nvarchar(256)
)
Go
--Bảng khuyến mãi (ưu đãi)
Create Table Promotion(
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	Description nvarchar(max),
	Discount float not null,
	CreateAt datetime not null default getDate(),
	ExpiryAt datetime not null,
)
Go
--Bảng đơn hàng
Create Table [Order](
	Id uniqueidentifier primary key,
	CustomerId uniqueidentifier foreign key references Customer(AccountId) not null,
	RentalTime int not null,
	UnitPrice float not null,
	DeliveryFee float,
	DeliveryDistance float,
	Deposit float not null,
	Amount float not null,
	PromotionId uniqueidentifier foreign key references Promotion(Id),
	IsPaid bit not null default 0,
	Status nvarchar(256) not null,
	Description nvarchar(max),
	CreateAt datetime not null default getDate()
)
Go
--Bảng chi tiết đơn hàng
Create Table OrderDetail(
	Id uniqueidentifier primary key,
	OrderId uniqueidentifier foreign key references [Order](Id),
	CarId uniqueidentifier foreign key references Car(Id),
	StartTime datetime not null,
	EndTime datetime not null,
	DeliveryLocationId uniqueidentifier foreign key references [Location](Id),
	PickUpLocationId uniqueidentifier foreign key references [Location](Id),
	DeliveryTime datetime not null,
	PickUpTime datetime not null,
	DriverId uniqueidentifier foreign key references Driver(AccountId),
)
Go
--Bảng feedback
Create Table [FeedBack](
	Id uniqueidentifier primary key,
	OrderId uniqueidentifier foreign key references [Order](Id)  not null,
	CustomerId uniqueidentifier foreign key references Customer(AccountId) not null,
	CarId uniqueidentifier foreign key references Car(Id),
	DriverId uniqueidentifier foreign key references Driver(AccountId),
	Star int not null,
	CreateAt datetime not null default getDate(),
	Content nvarchar(max)
)
Go
--Bảng để lưu url của ảnh (dùng chung cho nhiều đối tượng và chia theo loại (Type))
Create Table [Image](
	Id uniqueidentifier primary key,
	Url nvarchar(256) not null,
	Type nvarchar(256) not null,
	ShowroomId uniqueidentifier foreign key references Showroom(Id),
	CarId uniqueidentifier foreign key references Car(Id),
	CarRegistrationId uniqueidentifier foreign key references CarRegistration(Id),
	CustomerId uniqueidentifier foreign key references Customer(AccountId),
	DriverId uniqueidentifier foreign key references Driver(AccountId),
)
GO
