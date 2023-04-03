Use master
Go
Create Database CarRental
Go
Use CarRental
Go
Create Table Account(
	Id uniqueidentifier primary key,
	Username varchar(256) not null unique,
	Password varchar(256) not null,
	Status bit not null
)
Go
Create Table Wallet(
	Id uniqueidentifier primary key,
	Balance float not null default 0,
	Status nvarchar(256) not null
)
Go
Create Table [Location](
	Id uniqueidentifier primary key,
	Longitude float not null,
	Latitude float not null
)
Go
Create Table Customer(
	AccountId uniqueidentifier primary key references Account(Id) not null,
	Name nvarchar(256) not null,
	Address nvarchar(256),
	Phone varchar(256) not null unique,
	Gender nvarchar(256) not null,
	AvatarUrl nvarchar(max),
	BankAccountNumber nvarchar(256),
	BankName nvarchar(256),
	WalletId uniqueidentifier foreign key references Wallet(Id) not null unique,
)
Go
Create Table CarOwner(
	AccountId uniqueidentifier primary key references Account(Id) not null,
	Name nvarchar(256) not null,
	Address nvarchar(256),
	Phone varchar(256) not null unique,
	Gender nvarchar(256) not null,
	AvatarUrl nvarchar(max),
	BankAccountNumber nvarchar(256),
	BankName nvarchar(256),
	WalletId uniqueidentifier foreign key references Wallet(Id) not null unique,
)
Go
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
	Status nvarchar(256) not null
)
Go
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
Create Table DeviceToken(
	Id uniqueidentifier primary key,
	AccountId uniqueidentifier foreign key references Account(Id) not null,
	Token nvarchar(max),
	CreateAt datetime not null default getdate()
)
Go
Create Table Showroom(
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	Description nvarchar(max),
	LocationId uniqueidentifier foreign key references [Location](Id),
)
Go
Create Table ProductionCompany(
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	Description nvarchar(max),
)
Go
Create Table Model(
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	CeilingPrice float not null,
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
Create Table AdditionalCharge(
	Id uniqueidentifier primary key,
	MaximumDistance int not null,
	DistanceSurcharge float not null,
	TimeSurcharge float not null,
	AdditionalDistance float not null,
	AdditionalTime float not null
)
Go
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
Create Table Feature(
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	Description nvarchar(max),
)
Go
Create Table CarFeature(
	CarId uniqueidentifier foreign key references Car(Id),
	FeatureId uniqueidentifier foreign key references Feature(Id),
	Description nvarchar(max),
	Primary key (CarId, FeatureId)
)
Go
Create Table [Type](
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	Description nvarchar(max),
)
Go
Create Table CarType(
	CarId uniqueidentifier foreign key references Car(Id),
	TypeId uniqueidentifier foreign key references [Type](Id),
	Description nvarchar(max),
	Primary key (CarId, TypeId)
)
Go
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
Create Table Calendar(
	Id uniqueidentifier primary key,
	Weekday nvarchar(256) not null,
	StartTime time not null default '06:00:00',
	EndTime time not null default '22:00:00',
)
Go
Create Table CarCalendar(
	CalendarId uniqueidentifier foreign key references Calendar(Id) not null,
	CarId uniqueidentifier foreign key references Car(Id) not null,
	Description nvarchar(max),
	Primary key (CalendarId, CarId)
)
Go
Create Table CarRegistrationCalendar(
	CalendarId uniqueidentifier foreign key references Calendar(Id) not null,
	CarRegistrationId uniqueidentifier foreign key references CarRegistration(Id) not null,
	Description nvarchar(max),
	Primary key (CalendarId, CarRegistrationId)
)
Go
Create Table DriverCalendar(
	CalendarId uniqueidentifier foreign key references Calendar(Id) not null,
	CarId uniqueidentifier foreign key references Car(Id) not null,
	DriverId uniqueidentifier foreign key references Driver(AccountId) not null,
	Description nvarchar(max),
	Primary key (CalendarId, DriverId, CarId)
)
Go
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
Create Table Promotion(
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	Description nvarchar(max),
	Discount float not null,
	CreateAt datetime not null default getDate(),
	ExpiryAt datetime not null,
)
Go
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
Create Table [Image](
	Id uniqueidentifier primary key,
	Url nvarchar(256) not null,
	Type nvarchar(256) not null,
	ShowroomId uniqueidentifier foreign key references Showroom(Id),
	CarId uniqueidentifier foreign key references Car(Id),
	CarRegistrationId uniqueidentifier foreign key references CarRegistration(Id),
)
GO
INSERT [dbo].[Calendar] ([Id], [Weekday], [StartTime], [EndTime]) VALUES (N'd1cbac81-dd01-4449-b723-406361c3c540', N'Monday', CAST(N'08:00:00' AS Time), CAST(N'22:00:00' AS Time))
INSERT [dbo].[Calendar] ([Id], [Weekday], [StartTime], [EndTime]) VALUES (N'37634c75-f48b-4256-b8cc-49fd7581d3a0', N'Thursday', CAST(N'08:00:00' AS Time), CAST(N'20:00:00' AS Time))
GO
INSERT [dbo].[Account] ([Id], [Username], [Password], [Status]) VALUES (N'f6c89e0e-7261-45d1-ad58-23122b5567c8', N'carowner', N'carowner', 1)
INSERT [dbo].[Account] ([Id], [Username], [Password], [Status]) VALUES (N'd13fb3f2-6240-4304-b272-35595262f2f3', N'admin', N'admin', 1)
INSERT [dbo].[Account] ([Id], [Username], [Password], [Status]) VALUES (N'2eab62fb-8898-4f7c-9c99-4ce43bd88996', N'driver', N'driver', 1)
INSERT [dbo].[Account] ([Id], [Username], [Password], [Status]) VALUES (N'9f8a6e2e-b79f-4baf-8677-65ec40152658', N'manager', N'manager', 1)
INSERT [dbo].[Account] ([Id], [Username], [Password], [Status]) VALUES (N'8ad9b7c4-290b-484d-9217-6f5b9b736bd3', N'customer', N'customer', 1)
INSERT [dbo].[Account] ([Id], [Username], [Password], [Status]) VALUES (N'd5fa40fd-c9cc-421f-bfb5-707b9f6139b2', N'haidang', N'Haidang123', 1)
INSERT [dbo].[Account] ([Id], [Username], [Password], [Status]) VALUES (N'28c1235a-3bdf-41ae-9c17-74162e7257b7', N'asd', N'asd', 1)
INSERT [dbo].[Account] ([Id], [Username], [Password], [Status]) VALUES (N'ddd746c7-c70c-45f0-aaca-78a56a81c1bc', N'manager2', N'manager', 1)
INSERT [dbo].[Account] ([Id], [Username], [Password], [Status]) VALUES (N'c2a6059c-3f0f-4f15-9c84-7b074cd70835', N'loc123123', N'Tantai4899@@', 1)
INSERT [dbo].[Account] ([Id], [Username], [Password], [Status]) VALUES (N'ea306864-204c-45b0-8102-7ddb0ecac652', N'votantai555555', N'Tantai4899', 1)
INSERT [dbo].[Account] ([Id], [Username], [Password], [Status]) VALUES (N'e8d284cf-32fc-45e5-a921-9ef553c1750a', N'namque', N'Namque123', 1)
INSERT [dbo].[Account] ([Id], [Username], [Password], [Status]) VALUES (N'0cb3dee7-43d3-41b8-9eb4-bb970180c195', N'aaaa', N'qweqwe', 1)
INSERT [dbo].[Account] ([Id], [Username], [Password], [Status]) VALUES (N'a5dbf34d-8524-449b-bd65-c09ebebc2292', N'string', N'string', 1)
INSERT [dbo].[Account] ([Id], [Username], [Password], [Status]) VALUES (N'846c7407-3215-42f8-aead-ccfa1b279b85', N'manager5', N'123123', 1)
GO
INSERT [dbo].[Wallet] ([Id], [Balance], [Status]) VALUES (N'b5aab94d-f4df-4a46-80a9-030ce93fa20a', 0, N'Active')
INSERT [dbo].[Wallet] ([Id], [Balance], [Status]) VALUES (N'7324a193-f7c1-4748-8ffc-0654b581b7e4', 0, N'Active')
INSERT [dbo].[Wallet] ([Id], [Balance], [Status]) VALUES (N'9cc08fcc-361c-44a8-9432-132ebfe67dbd', 0, N'Active')
INSERT [dbo].[Wallet] ([Id], [Balance], [Status]) VALUES (N'306bcdfe-ac14-4a0a-adec-40dbcb64b850', 0, N'Active')
INSERT [dbo].[Wallet] ([Id], [Balance], [Status]) VALUES (N'e55e2b45-4033-44f4-b721-448ceeb632ac', 1000000, N'Active')
INSERT [dbo].[Wallet] ([Id], [Balance], [Status]) VALUES (N'38be23d3-7cbf-484f-9171-4d598289e40a', 0, N'Active')
INSERT [dbo].[Wallet] ([Id], [Balance], [Status]) VALUES (N'e71e369e-6cc3-4fab-9195-6aa35177aad7', 0, N'Active')
INSERT [dbo].[Wallet] ([Id], [Balance], [Status]) VALUES (N'e072f378-7446-4629-a03e-abe0de29ca21', 0, N'Active')
INSERT [dbo].[Wallet] ([Id], [Balance], [Status]) VALUES (N'5c8ab370-01b3-436d-9329-ae424bf3b2e5', 0, N'Active')
INSERT [dbo].[Wallet] ([Id], [Balance], [Status]) VALUES (N'a39c7824-b958-4365-8336-aeb8838a7c88', 0, N'Active')
INSERT [dbo].[Wallet] ([Id], [Balance], [Status]) VALUES (N'612f814c-2387-432b-9ba2-bf4c1d4ffd2d', 0, N'Active')
INSERT [dbo].[Wallet] ([Id], [Balance], [Status]) VALUES (N'fddf917c-ea09-40d8-b995-c6c3baf58308', 0, N'Active')
INSERT [dbo].[Wallet] ([Id], [Balance], [Status]) VALUES (N'd212e634-16b1-4bfc-b617-e6fc27ed8763', 0, N'Active')
INSERT [dbo].[Wallet] ([Id], [Balance], [Status]) VALUES (N'74668d25-18f4-48d1-8f48-f6a3c4a8b3d5', 0, N'Active')
GO
INSERT [dbo].[CarOwner] ([AccountId], [Name], [Address], [Phone], [Gender], [AvatarUrl], [BankAccountNumber], [BankName], [WalletId]) VALUES (N'f6c89e0e-7261-45d1-ad58-23122b5567c8', N'Car Owner', N'Car owner address', N'0345464757', N'Nam', NULL, NULL, NULL, N'b5aab94d-f4df-4a46-80a9-030ce93fa20a')
GO
INSERT [dbo].[Driver] ([AccountId], [Name], [Address], [Phone], [Gender], [AvatarUrl], [Star], [BankAccountNumber], [BankName], [WalletId], [LocationId], [Status]) VALUES (N'2eab62fb-8898-4f7c-9c99-4ce43bd88996', N'Lê Lâm Nhất Linh', N'Driver address', N'0345667887', N'Nam', NULL, NULL, NULL, NULL, N'38be23d3-7cbf-484f-9171-4d598289e40a', NULL, N'Idle')
GO
INSERT [dbo].[ProductionCompany] ([Id], [Name], [Description]) VALUES (N'1a91dd16-48ec-447f-ad9a-50b0d03c294d', N'Audi', NULL)
GO
INSERT [dbo].[Model] ([Id], [Name], [CeilingPrice], [FloorPrice], [Seater], [Chassis], [YearOfManufacture], [TransmissionType], [FuelType], [FuelConsumption], [ProductionCompanyId]) VALUES (N'b4379d91-48f9-48c7-8f2c-307f16154756', N'Chevrolet Camaro
', 100000, 240000, 4, N'Cao', 2011, N'2 cầu', N'Dầu', N'Thấp', N'1a91dd16-48ec-447f-ad9a-50b0d03c294d')
INSERT [dbo].[Model] ([Id], [Name], [CeilingPrice], [FloorPrice], [Seater], [Chassis], [YearOfManufacture], [TransmissionType], [FuelType], [FuelConsumption], [ProductionCompanyId]) VALUES (N'9b9505c0-6db3-4cfb-9ca4-97c0d5218dbb', N'Ford Mustang
', 100000, 200000, 8, N'Cao', 2019, N'2 cầu', N'Xăng', N'Nhiều', N'1a91dd16-48ec-447f-ad9a-50b0d03c294d')
INSERT [dbo].[Model] ([Id], [Name], [CeilingPrice], [FloorPrice], [Seater], [Chassis], [YearOfManufacture], [TransmissionType], [FuelType], [FuelConsumption], [ProductionCompanyId]) VALUES (N'293a52a0-97cf-471c-977c-ef240678bffb', N'Toyota Corolla
', 100000, 299999, 7, N'Thấp', 2018, N'1 cầu', N'Xăng', N'Trung Bình', N'1a91dd16-48ec-447f-ad9a-50b0d03c294d')
GO
INSERT [dbo].[AdditionalCharge] ([Id], [MaximumDistance], [DistanceSurcharge], [TimeSurcharge], [AdditionalDistance], [AdditionalTime]) VALUES (N'f99b6242-0b31-4790-a654-504b828bbc19', 100, 200000, 200000, 50, 24)
GO
INSERT [dbo].[Customer] ([AccountId], [Name], [Address], [Phone], [Gender], [AvatarUrl], [BankAccountNumber], [BankName], [WalletId]) VALUES (N'8ad9b7c4-290b-484d-9217-6f5b9b736bd3', N'Customer', N'Customer Address', N'0348040506', N'Nam', NULL, NULL, NULL, N'74668d25-18f4-48d1-8f48-f6a3c4a8b3d5')
INSERT [dbo].[Customer] ([AccountId], [Name], [Address], [Phone], [Gender], [AvatarUrl], [BankAccountNumber], [BankName], [WalletId]) VALUES (N'a5dbf34d-8524-449b-bd65-c09ebebc2292', N'Võ Văn Thế Tri', N'string', N'string', N'string', NULL, NULL, NULL, N'306bcdfe-ac14-4a0a-adec-40dbcb64b850')
GO
INSERT [dbo].[Notification] ([Id], [Title], [Body], [Type], [Link], [AccountId], [IsRead], [CreateAt]) VALUES (N'17a00b91-a1a5-4f08-b8b2-2d319091f961', N'1', N'1', N'Order', N'9cc47a0f-5cba-49d5-a174-ea2f264096da', N'd13fb3f2-6240-4304-b272-35595262f2f3', 0, CAST(N'2023-04-01T11:22:40.223' AS DateTime))
INSERT [dbo].[Notification] ([Id], [Title], [Body], [Type], [Link], [AccountId], [IsRead], [CreateAt]) VALUES (N'bb2a2ae6-d0d0-4dc4-a2cb-9b58ecfa215a', N'1', N'1', N'Order', N'981376c4-d5f5-4142-8cf1-ef998ddf8fcd', N'd13fb3f2-6240-4304-b272-35595262f2f3', 0, CAST(N'2023-04-01T11:23:51.347' AS DateTime))
GO
INSERT [dbo].[User] ([AccountId], [Name], [Phone], [Gender], [AvatarUrl], [Role], [WalletId]) VALUES (N'd13fb3f2-6240-4304-b272-35595262f2f3', N'Administrator', N'0339040899', N'Nam', NULL, N'Admin', N'e55e2b45-4033-44f4-b721-448ceeb632ac')
INSERT [dbo].[User] ([AccountId], [Name], [Phone], [Gender], [AvatarUrl], [Role], [WalletId]) VALUES (N'9f8a6e2e-b79f-4baf-8677-65ec40152658', N'Manager', N'0348040899', N'Nam', NULL, N'Manager', N'd212e634-16b1-4bfc-b617-e6fc27ed8763')
GO
INSERT [dbo].[DeviceToken] ([Id], [AccountId], [Token], [CreateAt]) VALUES (N'12e1451f-2af8-4441-b973-0deb05c75de5', N'9f8a6e2e-b79f-4baf-8677-65ec40152658', N'eYDzgvfC24fhwQjRx7EhWA:APA91bHyZHIsLOf2HMR4-ZbImHFzcAfcXL3TBZtKHiKLRei02belf0j01ziKdhDEs6_lP5lx0bDjxA4gdZV46P56p9rJb4Rj-yRW7ZES7EnKY-oLr1hwFCsTyhG7-Jz_yHCdgt7ZBJCh', CAST(N'2023-04-01T11:17:59.817' AS DateTime))
INSERT [dbo].[DeviceToken] ([Id], [AccountId], [Token], [CreateAt]) VALUES (N'09eb5d50-a960-4d5b-bede-0e439ac005cb', N'd13fb3f2-6240-4304-b272-35595262f2f3', N'eYDzgvfC24fhwQjRx7EhWA:APA91bHyZHIsLOf2HMR4-ZbImHFzcAfcXL3TBZtKHiKLRei02belf0j01ziKdhDEs6_lP5lx0bDjxA4gdZV46P56p9rJb4Rj-yRW7ZES7EnKY-oLr1hwFCsTyhG7-Jz_yHCdgt7ZBJCh', CAST(N'2023-04-01T11:03:34.200' AS DateTime))
GO
