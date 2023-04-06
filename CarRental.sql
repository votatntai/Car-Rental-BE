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
INSERT [dbo].[Calendar] ([Id], [Weekday], [StartTime], [EndTime]) VALUES (N'dbd0e2c6-22dc-414d-89a7-180a881c1421', N'Tuesday', CAST(N'08:00:00' AS Time), CAST(N'20:00:00' AS Time))
INSERT [dbo].[Calendar] ([Id], [Weekday], [StartTime], [EndTime]) VALUES (N'64f8c8c7-1670-4b72-86c0-275aedfdec81', N'Sunday', CAST(N'08:00:00' AS Time), CAST(N'20:00:00' AS Time))
INSERT [dbo].[Calendar] ([Id], [Weekday], [StartTime], [EndTime]) VALUES (N'16fdfbd1-6af4-4343-a386-38781ff116cd', N'Friday', CAST(N'08:00:00' AS Time), CAST(N'20:00:00' AS Time))
INSERT [dbo].[Calendar] ([Id], [Weekday], [StartTime], [EndTime]) VALUES (N'1454eeae-4462-4371-abf4-39c53093a363', N'Thursday', CAST(N'08:00:00' AS Time), CAST(N'20:00:00' AS Time))
INSERT [dbo].[Calendar] ([Id], [Weekday], [StartTime], [EndTime]) VALUES (N'331f4f4d-6bb5-4c3a-889b-3fe723270195', N'Saturday', CAST(N'08:00:00' AS Time), CAST(N'20:00:00' AS Time))
INSERT [dbo].[Calendar] ([Id], [Weekday], [StartTime], [EndTime]) VALUES (N'd1cbac81-dd01-4449-b723-406361c3c540', N'Monday', CAST(N'08:00:00' AS Time), CAST(N'22:00:00' AS Time))
INSERT [dbo].[Calendar] ([Id], [Weekday], [StartTime], [EndTime]) VALUES (N'37634c75-f48b-4256-b8cc-49fd7581d3a0', N'Thursday', CAST(N'08:00:00' AS Time), CAST(N'20:00:00' AS Time))
INSERT [dbo].[Calendar] ([Id], [Weekday], [StartTime], [EndTime]) VALUES (N'cf8f83b0-a3c5-4263-aad4-4c636d48925b', N'Wednesday', CAST(N'08:00:00' AS Time), CAST(N'20:00:00' AS Time))
INSERT [dbo].[Calendar] ([Id], [Weekday], [StartTime], [EndTime]) VALUES (N'ffbf9e00-4451-4ec5-a1c9-5d0a580717c7', N'Sunday', CAST(N'08:00:00' AS Time), CAST(N'20:00:00' AS Time))
INSERT [dbo].[Calendar] ([Id], [Weekday], [StartTime], [EndTime]) VALUES (N'03dd74d3-8075-4ce5-a7d9-5d2757478e4d', N'Thursday', CAST(N'08:00:00' AS Time), CAST(N'20:00:00' AS Time))
INSERT [dbo].[Calendar] ([Id], [Weekday], [StartTime], [EndTime]) VALUES (N'9ace9f19-21ed-4555-bd26-83b4de11c79e', N'Thursday', CAST(N'08:00:00' AS Time), CAST(N'20:00:00' AS Time))
INSERT [dbo].[Calendar] ([Id], [Weekday], [StartTime], [EndTime]) VALUES (N'7e7ccf01-28d1-48e2-924b-89e84c803c26', N'Sunday', CAST(N'08:00:00' AS Time), CAST(N'20:00:00' AS Time))
INSERT [dbo].[Calendar] ([Id], [Weekday], [StartTime], [EndTime]) VALUES (N'32e47b7f-add2-4293-b65a-993e89cc2a89', N'Monday', CAST(N'08:00:00' AS Time), CAST(N'20:00:00' AS Time))
INSERT [dbo].[Calendar] ([Id], [Weekday], [StartTime], [EndTime]) VALUES (N'd3c667a2-af37-400d-a4f4-9c758fed01f2', N'Saturday', CAST(N'08:00:00' AS Time), CAST(N'20:00:00' AS Time))
INSERT [dbo].[Calendar] ([Id], [Weekday], [StartTime], [EndTime]) VALUES (N'c6a74192-b4a5-4ef7-9c18-aa56ef7e56f2', N'Wednesday', CAST(N'08:00:00' AS Time), CAST(N'20:00:00' AS Time))
INSERT [dbo].[Calendar] ([Id], [Weekday], [StartTime], [EndTime]) VALUES (N'b9ef7306-e616-4902-9e67-b1deed6ebc7b', N'Wednesday', CAST(N'08:00:00' AS Time), CAST(N'20:00:00' AS Time))
INSERT [dbo].[Calendar] ([Id], [Weekday], [StartTime], [EndTime]) VALUES (N'45505457-26ee-44e4-8775-b23fb31b2a8b', N'Monday', CAST(N'08:00:00' AS Time), CAST(N'20:00:00' AS Time))
INSERT [dbo].[Calendar] ([Id], [Weekday], [StartTime], [EndTime]) VALUES (N'ffa14c00-4595-48b9-8428-b587375d71a1', N'Monday', CAST(N'08:00:00' AS Time), CAST(N'20:00:00' AS Time))
INSERT [dbo].[Calendar] ([Id], [Weekday], [StartTime], [EndTime]) VALUES (N'640f898a-d003-4aa0-a1f1-beb7c2e9e273', N'Saturday', CAST(N'08:00:00' AS Time), CAST(N'20:00:00' AS Time))
INSERT [dbo].[Calendar] ([Id], [Weekday], [StartTime], [EndTime]) VALUES (N'a2aa9d83-4e51-4a21-81c5-c7a8b5dae59f', N'Friday', CAST(N'08:00:00' AS Time), CAST(N'20:00:00' AS Time))
INSERT [dbo].[Calendar] ([Id], [Weekday], [StartTime], [EndTime]) VALUES (N'64d4db89-10d5-490d-952d-d1ba4bc735c5', N'Friday', CAST(N'08:00:00' AS Time), CAST(N'20:00:00' AS Time))
INSERT [dbo].[Calendar] ([Id], [Weekday], [StartTime], [EndTime]) VALUES (N'dd51de01-e4ea-4df0-afba-eadb54896de8', N'Tuesday', CAST(N'08:00:00' AS Time), CAST(N'20:00:00' AS Time))
INSERT [dbo].[Calendar] ([Id], [Weekday], [StartTime], [EndTime]) VALUES (N'3b9a4654-0608-473c-8acf-fa767f3e0dc2', N'Tuesday', CAST(N'08:00:00' AS Time), CAST(N'20:00:00' AS Time))
GO
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'81121bd1-80dc-4546-803a-3f167fdb71f8', 106.7911434173584, 10.847847557628176)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'51d10f5d-dba0-41f9-ab22-433ff7d25094', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'582f96e2-1565-452e-b509-51b2832f416d', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'423e58d6-6bba-48e9-8a6b-5552cbbbd6bc', 106.79719, 10.8409534)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'c276df73-39e2-4c58-826b-5dd68583c67d', 108.46288248896599, 11.947169686541477)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'a548da6b-b60c-4da4-8420-644cf40a0dcb', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'b7978a09-0a5a-435e-b86d-7610cd6b13a0', 10.123124234, 12.12312422)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'4494b226-f4b9-4550-940c-93e16ddae937', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'0bff7d37-6cf0-4e6a-803e-d6e3579ee918', 106.81268692016602, 10.840513634735517)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'637ea054-8a58-4c2f-ad49-e042cc246447', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'53867e5f-3656-416e-9b7b-e7ac5f5a0a04', 106.81268692016602, 10.840513634735517)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'e7013491-c6fc-4613-bab0-ede72cf28555', 0, 0)
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
INSERT [dbo].[AdditionalCharge] ([Id], [MaximumDistance], [DistanceSurcharge], [TimeSurcharge], [AdditionalDistance], [AdditionalTime]) VALUES (N'7da4a884-0904-46cf-a15d-05540b348336', 100, 100, 2, 12000, 20000)
INSERT [dbo].[AdditionalCharge] ([Id], [MaximumDistance], [DistanceSurcharge], [TimeSurcharge], [AdditionalDistance], [AdditionalTime]) VALUES (N'2cd134ab-3127-4143-bdb0-31325bbbc470', 100, 100, 2, 12000, 20000)
INSERT [dbo].[AdditionalCharge] ([Id], [MaximumDistance], [DistanceSurcharge], [TimeSurcharge], [AdditionalDistance], [AdditionalTime]) VALUES (N'488a0cbd-27fc-487f-aeed-3c9c69df2928', 100, 100, 2, 12000, 20000)
INSERT [dbo].[AdditionalCharge] ([Id], [MaximumDistance], [DistanceSurcharge], [TimeSurcharge], [AdditionalDistance], [AdditionalTime]) VALUES (N'f99b6242-0b31-4790-a654-504b828bbc19', 100, 200000, 200000, 50, 24)
INSERT [dbo].[AdditionalCharge] ([Id], [MaximumDistance], [DistanceSurcharge], [TimeSurcharge], [AdditionalDistance], [AdditionalTime]) VALUES (N'fb28548d-b2d4-4c56-a79d-6d2d7716b2aa', 100, 100, 2, 12000, 20000)
GO
INSERT [dbo].[Car] ([Id], [Name], [LicensePlate], [Price], [CreateAt], [Description], [ModelId], [LocationId], [AdditionalChargeId], [DriverId], [CarOwnerId], [ShowroomId], [Rented], [ReceiveStartTime], [ReceiveEndTime], [ReturnStartTime], [ReturnEndTime], [Star], [IsTracking], [Status]) VALUES (N'364d3bed-99ef-4abc-9451-0bc82429d9e4', N'Car Registration', N'71B3.54324', 1000000, CAST(N'2023-04-04T11:48:07.843' AS DateTime), NULL, N'b4379d91-48f9-48c7-8f2c-307f16154756', N'0bff7d37-6cf0-4e6a-803e-d6e3579ee918', N'7da4a884-0904-46cf-a15d-05540b348336', NULL, N'f6c89e0e-7261-45d1-ad58-23122b5567c8', NULL, 0, CAST(N'06:00:00' AS Time), CAST(N'22:00:00' AS Time), CAST(N'06:00:00' AS Time), CAST(N'22:00:00' AS Time), NULL, 1, N'Idle')
INSERT [dbo].[Car] ([Id], [Name], [LicensePlate], [Price], [CreateAt], [Description], [ModelId], [LocationId], [AdditionalChargeId], [DriverId], [CarOwnerId], [ShowroomId], [Rented], [ReceiveStartTime], [ReceiveEndTime], [ReturnStartTime], [ReturnEndTime], [Star], [IsTracking], [Status]) VALUES (N'11080eb4-442e-4963-9007-148b436e43bb', N'Car Registration 2', N'71B3.54324', 1000000, CAST(N'2023-04-04T18:54:45.450' AS DateTime), NULL, N'293a52a0-97cf-471c-977c-ef240678bffb', N'c276df73-39e2-4c58-826b-5dd68583c67d', N'2cd134ab-3127-4143-bdb0-31325bbbc470', NULL, N'f6c89e0e-7261-45d1-ad58-23122b5567c8', NULL, 0, CAST(N'06:00:00' AS Time), CAST(N'22:00:00' AS Time), CAST(N'06:00:00' AS Time), CAST(N'22:00:00' AS Time), NULL, 0, N'Idle')
INSERT [dbo].[Car] ([Id], [Name], [LicensePlate], [Price], [CreateAt], [Description], [ModelId], [LocationId], [AdditionalChargeId], [DriverId], [CarOwnerId], [ShowroomId], [Rented], [ReceiveStartTime], [ReceiveEndTime], [ReturnStartTime], [ReturnEndTime], [Star], [IsTracking], [Status]) VALUES (N'e867a60b-1dd5-4092-b9d3-90ba0b3d628a', N'Car Registration 2', N'71B3.54324', 1000000, CAST(N'2023-04-04T11:53:28.167' AS DateTime), NULL, N'b4379d91-48f9-48c7-8f2c-307f16154756', N'81121bd1-80dc-4546-803a-3f167fdb71f8', N'488a0cbd-27fc-487f-aeed-3c9c69df2928', NULL, N'f6c89e0e-7261-45d1-ad58-23122b5567c8', NULL, 0, CAST(N'06:00:00' AS Time), CAST(N'22:00:00' AS Time), CAST(N'06:00:00' AS Time), CAST(N'22:00:00' AS Time), NULL, 0, N'Idle')
INSERT [dbo].[Car] ([Id], [Name], [LicensePlate], [Price], [CreateAt], [Description], [ModelId], [LocationId], [AdditionalChargeId], [DriverId], [CarOwnerId], [ShowroomId], [Rented], [ReceiveStartTime], [ReceiveEndTime], [ReturnStartTime], [ReturnEndTime], [Star], [IsTracking], [Status]) VALUES (N'f9c0fe2b-3e71-4622-ad27-98c7f9d6d2c9', N'Xe Rồng', N'64T2.15674', 10000, CAST(N'2023-04-03T18:41:20.073' AS DateTime), NULL, N'b4379d91-48f9-48c7-8f2c-307f16154756', N'b7978a09-0a5a-435e-b86d-7610cd6b13a0', N'f99b6242-0b31-4790-a654-504b828bbc19', N'2eab62fb-8898-4f7c-9c99-4ce43bd88996', N'f6c89e0e-7261-45d1-ad58-23122b5567c8', NULL, 12, CAST(N'06:00:00' AS Time), CAST(N'22:00:00' AS Time), CAST(N'06:00:00' AS Time), CAST(N'22:00:00' AS Time), NULL, 0, N'Idle')
GO
INSERT [dbo].[CarCalendar] ([CalendarId], [CarId], [Description]) VALUES (N'dbd0e2c6-22dc-414d-89a7-180a881c1421', N'364d3bed-99ef-4abc-9451-0bc82429d9e4', NULL)
INSERT [dbo].[CarCalendar] ([CalendarId], [CarId], [Description]) VALUES (N'64f8c8c7-1670-4b72-86c0-275aedfdec81', N'e867a60b-1dd5-4092-b9d3-90ba0b3d628a', NULL)
INSERT [dbo].[CarCalendar] ([CalendarId], [CarId], [Description]) VALUES (N'16fdfbd1-6af4-4343-a386-38781ff116cd', N'11080eb4-442e-4963-9007-148b436e43bb', NULL)
INSERT [dbo].[CarCalendar] ([CalendarId], [CarId], [Description]) VALUES (N'1454eeae-4462-4371-abf4-39c53093a363', N'364d3bed-99ef-4abc-9451-0bc82429d9e4', NULL)
INSERT [dbo].[CarCalendar] ([CalendarId], [CarId], [Description]) VALUES (N'331f4f4d-6bb5-4c3a-889b-3fe723270195', N'364d3bed-99ef-4abc-9451-0bc82429d9e4', NULL)
INSERT [dbo].[CarCalendar] ([CalendarId], [CarId], [Description]) VALUES (N'cf8f83b0-a3c5-4263-aad4-4c636d48925b', N'11080eb4-442e-4963-9007-148b436e43bb', NULL)
INSERT [dbo].[CarCalendar] ([CalendarId], [CarId], [Description]) VALUES (N'ffbf9e00-4451-4ec5-a1c9-5d0a580717c7', N'364d3bed-99ef-4abc-9451-0bc82429d9e4', NULL)
INSERT [dbo].[CarCalendar] ([CalendarId], [CarId], [Description]) VALUES (N'03dd74d3-8075-4ce5-a7d9-5d2757478e4d', N'11080eb4-442e-4963-9007-148b436e43bb', NULL)
INSERT [dbo].[CarCalendar] ([CalendarId], [CarId], [Description]) VALUES (N'9ace9f19-21ed-4555-bd26-83b4de11c79e', N'e867a60b-1dd5-4092-b9d3-90ba0b3d628a', NULL)
INSERT [dbo].[CarCalendar] ([CalendarId], [CarId], [Description]) VALUES (N'7e7ccf01-28d1-48e2-924b-89e84c803c26', N'11080eb4-442e-4963-9007-148b436e43bb', NULL)
INSERT [dbo].[CarCalendar] ([CalendarId], [CarId], [Description]) VALUES (N'32e47b7f-add2-4293-b65a-993e89cc2a89', N'11080eb4-442e-4963-9007-148b436e43bb', NULL)
INSERT [dbo].[CarCalendar] ([CalendarId], [CarId], [Description]) VALUES (N'd3c667a2-af37-400d-a4f4-9c758fed01f2', N'11080eb4-442e-4963-9007-148b436e43bb', NULL)
INSERT [dbo].[CarCalendar] ([CalendarId], [CarId], [Description]) VALUES (N'c6a74192-b4a5-4ef7-9c18-aa56ef7e56f2', N'e867a60b-1dd5-4092-b9d3-90ba0b3d628a', NULL)
INSERT [dbo].[CarCalendar] ([CalendarId], [CarId], [Description]) VALUES (N'b9ef7306-e616-4902-9e67-b1deed6ebc7b', N'364d3bed-99ef-4abc-9451-0bc82429d9e4', NULL)
INSERT [dbo].[CarCalendar] ([CalendarId], [CarId], [Description]) VALUES (N'45505457-26ee-44e4-8775-b23fb31b2a8b', N'364d3bed-99ef-4abc-9451-0bc82429d9e4', NULL)
INSERT [dbo].[CarCalendar] ([CalendarId], [CarId], [Description]) VALUES (N'ffa14c00-4595-48b9-8428-b587375d71a1', N'e867a60b-1dd5-4092-b9d3-90ba0b3d628a', NULL)
INSERT [dbo].[CarCalendar] ([CalendarId], [CarId], [Description]) VALUES (N'640f898a-d003-4aa0-a1f1-beb7c2e9e273', N'e867a60b-1dd5-4092-b9d3-90ba0b3d628a', NULL)
INSERT [dbo].[CarCalendar] ([CalendarId], [CarId], [Description]) VALUES (N'a2aa9d83-4e51-4a21-81c5-c7a8b5dae59f', N'e867a60b-1dd5-4092-b9d3-90ba0b3d628a', NULL)
INSERT [dbo].[CarCalendar] ([CalendarId], [CarId], [Description]) VALUES (N'64d4db89-10d5-490d-952d-d1ba4bc735c5', N'364d3bed-99ef-4abc-9451-0bc82429d9e4', NULL)
INSERT [dbo].[CarCalendar] ([CalendarId], [CarId], [Description]) VALUES (N'dd51de01-e4ea-4df0-afba-eadb54896de8', N'e867a60b-1dd5-4092-b9d3-90ba0b3d628a', NULL)
INSERT [dbo].[CarCalendar] ([CalendarId], [CarId], [Description]) VALUES (N'3b9a4654-0608-473c-8acf-fa767f3e0dc2', N'11080eb4-442e-4963-9007-148b436e43bb', NULL)
GO
INSERT [dbo].[CarRegistration] ([Id], [Name], [LicensePlate], [TransmissionType], [FuelType], [Seater], [Price], [FuelConsumption], [YearOfManufacture], [ProductionCompany], [Model], [Chassis], [Location], [CreateAt], [CarOwnerId], [AdditionalChargeId], [Description], [Status]) VALUES (N'2d5f45b2-f266-4956-9ab3-8cac4280f4a7', N'Car Registration 2', N'71B3.54324', N'2 cầu', N'Xăng', 7, 1000000, N'Cao', 2022, N'Evericks', N'G64', N'Cao', N'Phố đi bộ Nguyễn Huệ', CAST(N'2023-04-04T11:45:21.583' AS DateTime), N'f6c89e0e-7261-45d1-ad58-23122b5567c8', N'fb28548d-b2d4-4c56-a79d-6d2d7716b2aa', N'', 1)
GO
INSERT [dbo].[Customer] ([AccountId], [Name], [Address], [Phone], [Gender], [AvatarUrl], [BankAccountNumber], [BankName], [WalletId]) VALUES (N'8ad9b7c4-290b-484d-9217-6f5b9b736bd3', N'Customer', N'45 chuong duong', N'0348040506', N'male', NULL, NULL, NULL, N'74668d25-18f4-48d1-8f48-f6a3c4a8b3d5')
INSERT [dbo].[Customer] ([AccountId], [Name], [Address], [Phone], [Gender], [AvatarUrl], [BankAccountNumber], [BankName], [WalletId]) VALUES (N'a5dbf34d-8524-449b-bd65-c09ebebc2292', N'Võ Văn Thế Tri', N'string', N'string', N'string', NULL, NULL, NULL, N'306bcdfe-ac14-4a0a-adec-40dbcb64b850')
GO
INSERT [dbo].[Order] ([Id], [CustomerId], [RentalTime], [UnitPrice], [DeliveryFee], [DeliveryDistance], [Deposit], [Amount], [PromotionId], [IsPaid], [Status], [Description], [CreateAt]) VALUES (N'edbdfea2-bc59-4411-8eec-071fa6372967', N'8ad9b7c4-290b-484d-9217-6f5b9b736bd3', 1, 1000000, 61780, 3.0889999866485596, 318534, 0, NULL, 1, N'Finished', NULL, CAST(N'2023-04-04T11:31:08.313' AS DateTime))
INSERT [dbo].[Order] ([Id], [CustomerId], [RentalTime], [UnitPrice], [DeliveryFee], [DeliveryDistance], [Deposit], [Amount], [PromotionId], [IsPaid], [Status], [Description], [CreateAt]) VALUES (N'd637b48c-0bba-4224-9315-1803f1e9e0fe', N'a5dbf34d-8524-449b-bd65-c09ebebc2292', 2, 100000, 10000, 10, 30000, 0, NULL, 0, N'Finished', NULL, CAST(N'2023-04-03T11:42:11.093' AS DateTime))
INSERT [dbo].[Order] ([Id], [CustomerId], [RentalTime], [UnitPrice], [DeliveryFee], [DeliveryDistance], [Deposit], [Amount], [PromotionId], [IsPaid], [Status], [Description], [CreateAt]) VALUES (N'72f855f1-b411-4fb8-8df4-b00e9ff7d1b6', N'a5dbf34d-8524-449b-bd65-c09ebebc2292', 2, 100000, 10000, 10, 30000, 0, NULL, 0, N'Canceled', N'Ngu vc', CAST(N'2023-04-04T06:14:59.010' AS DateTime))
INSERT [dbo].[Order] ([Id], [CustomerId], [RentalTime], [UnitPrice], [DeliveryFee], [DeliveryDistance], [Deposit], [Amount], [PromotionId], [IsPaid], [Status], [Description], [CreateAt]) VALUES (N'6e786109-c4e4-4cf0-8d7f-e50647311622', N'a5dbf34d-8524-449b-bd65-c09ebebc2292', 2, 100000, 10000, 10, 30000, 0, NULL, 0, N'ArrivedAtPickUpPoint', NULL, CAST(N'2023-04-04T05:34:41.363' AS DateTime))
GO
INSERT [dbo].[OrderDetail] ([Id], [OrderId], [CarId], [StartTime], [EndTime], [DeliveryLocationId], [PickUpLocationId], [DeliveryTime], [PickUpTime], [DriverId]) VALUES (N'1e848b3f-b682-4b9e-8cf4-2b4e549a09ee', N'72f855f1-b411-4fb8-8df4-b00e9ff7d1b6', N'f9c0fe2b-3e71-4622-ad27-98c7f9d6d2c9', CAST(N'2023-04-03T11:35:47.497' AS DateTime), CAST(N'2023-04-03T11:35:47.497' AS DateTime), N'4494b226-f4b9-4550-940c-93e16ddae937', N'a548da6b-b60c-4da4-8420-644cf40a0dcb', CAST(N'2023-04-03T11:35:47.497' AS DateTime), CAST(N'2023-04-03T11:35:47.497' AS DateTime), N'2eab62fb-8898-4f7c-9c99-4ce43bd88996')
INSERT [dbo].[OrderDetail] ([Id], [OrderId], [CarId], [StartTime], [EndTime], [DeliveryLocationId], [PickUpLocationId], [DeliveryTime], [PickUpTime], [DriverId]) VALUES (N'5fed342a-2989-4760-979c-6783f05c3baf', N'6e786109-c4e4-4cf0-8d7f-e50647311622', N'f9c0fe2b-3e71-4622-ad27-98c7f9d6d2c9', CAST(N'2023-04-03T11:35:47.497' AS DateTime), CAST(N'2023-04-03T11:35:47.497' AS DateTime), N'637ea054-8a58-4c2f-ad49-e042cc246447', N'e7013491-c6fc-4613-bab0-ede72cf28555', CAST(N'2023-04-03T11:35:47.497' AS DateTime), CAST(N'2023-04-03T11:35:47.497' AS DateTime), N'2eab62fb-8898-4f7c-9c99-4ce43bd88996')
INSERT [dbo].[OrderDetail] ([Id], [OrderId], [CarId], [StartTime], [EndTime], [DeliveryLocationId], [PickUpLocationId], [DeliveryTime], [PickUpTime], [DriverId]) VALUES (N'107568d2-e34e-4e8d-b896-7a4189e5eeb1', N'd637b48c-0bba-4224-9315-1803f1e9e0fe', N'f9c0fe2b-3e71-4622-ad27-98c7f9d6d2c9', CAST(N'2023-04-03T11:35:47.497' AS DateTime), CAST(N'2023-04-03T11:35:47.497' AS DateTime), N'51d10f5d-dba0-41f9-ab22-433ff7d25094', N'582f96e2-1565-452e-b509-51b2832f416d', CAST(N'2023-04-03T11:35:47.497' AS DateTime), CAST(N'2023-04-03T11:35:47.497' AS DateTime), N'2eab62fb-8898-4f7c-9c99-4ce43bd88996')
INSERT [dbo].[OrderDetail] ([Id], [OrderId], [CarId], [StartTime], [EndTime], [DeliveryLocationId], [PickUpLocationId], [DeliveryTime], [PickUpTime], [DriverId]) VALUES (N'39dbed31-d737-48e9-a1f7-9424bff718be', N'edbdfea2-bc59-4411-8eec-071fa6372967', N'364d3bed-99ef-4abc-9451-0bc82429d9e4', CAST(N'2023-04-04T08:00:00.000' AS DateTime), CAST(N'2023-04-05T08:00:00.000' AS DateTime), N'423e58d6-6bba-48e9-8a6b-5552cbbbd6bc', N'53867e5f-3656-416e-9b7b-e7ac5f5a0a04', CAST(N'2023-04-04T08:00:00.000' AS DateTime), CAST(N'2023-04-04T08:00:00.000' AS DateTime), N'2eab62fb-8898-4f7c-9c99-4ce43bd88996')
GO
INSERT [dbo].[Notification] ([Id], [Title], [Body], [Type], [Link], [AccountId], [IsRead], [CreateAt]) VALUES (N'6915b451-9ceb-44c1-9c0d-047752df7595', N'Tài xế đã đến điểm đón', N'Tài xế của bạn đã đến điểm đón', N'Order', N'd637b48c-0bba-4224-9315-1803f1e9e0fe', N'a5dbf34d-8524-449b-bd65-c09ebebc2292', 0, CAST(N'2023-04-04T19:09:49.947' AS DateTime))
INSERT [dbo].[Notification] ([Id], [Title], [Body], [Type], [Link], [AccountId], [IsRead], [CreateAt]) VALUES (N'40900764-f668-4748-8c94-127a1fe0c8fe', N'Đơn hàng đã được chấp nhận', N'Đơn đặt hàng của bạn đã được chủ xe chấp nhận', N'Order', N'edbdfea2-bc59-4411-8eec-071fa6372967', N'8ad9b7c4-290b-484d-9217-6f5b9b736bd3', 0, CAST(N'2023-04-04T18:39:36.367' AS DateTime))
INSERT [dbo].[Notification] ([Id], [Title], [Body], [Type], [Link], [AccountId], [IsRead], [CreateAt]) VALUES (N'a4e7e270-b56e-41c4-b33d-26b4f57c2a96', N'Đơn hàng đã được tiến hành', N'Đơn hàng của bạn đang được thực hiện', N'Order', N'd637b48c-0bba-4224-9315-1803f1e9e0fe', N'a5dbf34d-8524-449b-bd65-c09ebebc2292', 0, CAST(N'2023-04-04T19:10:01.420' AS DateTime))
INSERT [dbo].[Notification] ([Id], [Title], [Body], [Type], [Link], [AccountId], [IsRead], [CreateAt]) VALUES (N'f56685bd-4a35-42ea-b122-3b1740a91d80', N'Đơn hàng đã được chấp nhận', N'Đơn đặt hàng của bạn đã được chủ xe chấp nhận', N'Order', N'd637b48c-0bba-4224-9315-1803f1e9e0fe', N'a5dbf34d-8524-449b-bd65-c09ebebc2292', 0, CAST(N'2023-04-04T19:08:07.710' AS DateTime))
INSERT [dbo].[Notification] ([Id], [Title], [Body], [Type], [Link], [AccountId], [IsRead], [CreateAt]) VALUES (N'8e494b8d-589d-44de-90c6-408c2a4639bf', N'Đơn hàng đã được tiến hành', N'Đơn hàng của bạn đang được thực hiện', N'Order', N'edbdfea2-bc59-4411-8eec-071fa6372967', N'8ad9b7c4-290b-484d-9217-6f5b9b736bd3', 0, CAST(N'2023-04-04T18:40:13.670' AS DateTime))
INSERT [dbo].[Notification] ([Id], [Title], [Body], [Type], [Link], [AccountId], [IsRead], [CreateAt]) VALUES (N'54805b2c-404b-4764-bd59-4cc1560e4547', N'Đơn hàng đã được tiến hành', N'Đơn hàng của bạn đang được thực hiện', N'Order', N'edbdfea2-bc59-4411-8eec-071fa6372967', N'f6c89e0e-7261-45d1-ad58-23122b5567c8', 0, CAST(N'2023-04-04T18:40:13.670' AS DateTime))
INSERT [dbo].[Notification] ([Id], [Title], [Body], [Type], [Link], [AccountId], [IsRead], [CreateAt]) VALUES (N'1516d033-8f3d-4510-83ef-803227f88851', N'Đơn hàng mới', N'Bạn có đơn hàng mới cần xác nhận', N'Order', N'edbdfea2-bc59-4411-8eec-071fa6372967', N'9f8a6e2e-b79f-4baf-8677-65ec40152658', 0, CAST(N'2023-04-04T18:31:08.420' AS DateTime))
INSERT [dbo].[Notification] ([Id], [Title], [Body], [Type], [Link], [AccountId], [IsRead], [CreateAt]) VALUES (N'bd90c1f5-5d90-47ee-9117-a07e74396a19', N'Đơn hàng mới', N'Bạn có đơn hàng mới cần xác nhận', N'Order', N'edbdfea2-bc59-4411-8eec-071fa6372967', N'f6c89e0e-7261-45d1-ad58-23122b5567c8', 0, CAST(N'2023-04-04T18:36:38.687' AS DateTime))
INSERT [dbo].[Notification] ([Id], [Title], [Body], [Type], [Link], [AccountId], [IsRead], [CreateAt]) VALUES (N'0407121e-f51a-4622-a2fa-e1cd3b4d2b8e', N'Đơn hàng đã được tiến hành', N'Đơn hàng của bạn đang được thực hiện', N'Order', N'd637b48c-0bba-4224-9315-1803f1e9e0fe', N'f6c89e0e-7261-45d1-ad58-23122b5567c8', 0, CAST(N'2023-04-04T19:10:01.420' AS DateTime))
GO
INSERT [dbo].[User] ([AccountId], [Name], [Phone], [Gender], [AvatarUrl], [Role], [WalletId]) VALUES (N'd13fb3f2-6240-4304-b272-35595262f2f3', N'Administrator', N'0339040899', N'Nam', NULL, N'Admin', N'e55e2b45-4033-44f4-b721-448ceeb632ac')
INSERT [dbo].[User] ([AccountId], [Name], [Phone], [Gender], [AvatarUrl], [Role], [WalletId]) VALUES (N'9f8a6e2e-b79f-4baf-8677-65ec40152658', N'Manager', N'0348040898', N'Nam', NULL, N'Manager', N'd212e634-16b1-4bfc-b617-e6fc27ed8763')
GO
INSERT [dbo].[DeviceToken] ([Id], [AccountId], [Token], [CreateAt]) VALUES (N'a6aa0407-7c45-4379-8f99-00ace01ee281', N'f6c89e0e-7261-45d1-ad58-23122b5567c8', N'cWYEQTJZR1eKlUd-W3qzeD:APA91bGhhq-6Ub7p1yIV1wFh6VRXCLXeSRZOC-sJga-dBqXxDZC_SaxWm57AMTB1du-NPYlpKxUIPpM2IvvlKKwA15Si8LttpxEkTd_gNKJtnnpH5QzVXuzTK74wh4XXSLcibz9f-IJo', CAST(N'2023-04-04T18:36:16.857' AS DateTime))
INSERT [dbo].[DeviceToken] ([Id], [AccountId], [Token], [CreateAt]) VALUES (N'12e1451f-2af8-4441-b973-0deb05c75de5', N'9f8a6e2e-b79f-4baf-8677-65ec40152658', N'eYDzgvfC24fhwQjRx7EhWA:APA91bHyZHIsLOf2HMR4-ZbImHFzcAfcXL3TBZtKHiKLRei02belf0j01ziKdhDEs6_lP5lx0bDjxA4gdZV46P56p9rJb4Rj-yRW7ZES7EnKY-oLr1hwFCsTyhG7-Jz_yHCdgt7ZBJCh', CAST(N'2023-04-01T11:17:59.817' AS DateTime))
INSERT [dbo].[DeviceToken] ([Id], [AccountId], [Token], [CreateAt]) VALUES (N'09eb5d50-a960-4d5b-bede-0e439ac005cb', N'd13fb3f2-6240-4304-b272-35595262f2f3', N'eYDzgvfC24fhwQjRx7EhWA:APA91bHyZHIsLOf2HMR4-ZbImHFzcAfcXL3TBZtKHiKLRei02belf0j01ziKdhDEs6_lP5lx0bDjxA4gdZV46P56p9rJb4Rj-yRW7ZES7EnKY-oLr1hwFCsTyhG7-Jz_yHCdgt7ZBJCh', CAST(N'2023-04-01T11:03:34.200' AS DateTime))
INSERT [dbo].[DeviceToken] ([Id], [AccountId], [Token], [CreateAt]) VALUES (N'85be0633-9aa8-4ea7-9943-753dd94e469d', N'8ad9b7c4-290b-484d-9217-6f5b9b736bd3', N'c5q4FK2NQ0ax3-eCnQNxxj:APA91bGDnDe4oS_xRTf18xf_FbRvaNjel7rY-82uDae4pHP15a02qqXgfE8ZOGHj5yebfVQw7EZdUbF-vkm7tIkM_Tfrmn_ePxsjw1issdjasIi6NEcdvHmpCrMwThMrkyyvPPcpl8zL', CAST(N'2023-04-04T18:27:57.970' AS DateTime))
INSERT [dbo].[DeviceToken] ([Id], [AccountId], [Token], [CreateAt]) VALUES (N'4f57dc25-fff8-402c-bfda-83dfa616c88a', N'9f8a6e2e-b79f-4baf-8677-65ec40152658', N'eYDzgvfC24fhwQjRx7EhWA:APA91bEheSrIiwigGhr3-Yzd1oXtvzqGM91Xb2qYrbre1g2-YUuXbMm0e8kCZcYdHQuzP4TfZi4RoFGZ_6UW3n7WW93S9njBA5n04o8smaSewliAAANhURhNDN_S3mkoXvfKXAt7F1ut', CAST(N'2023-04-03T17:56:26.927' AS DateTime))
INSERT [dbo].[DeviceToken] ([Id], [AccountId], [Token], [CreateAt]) VALUES (N'e91847bd-2e9a-419c-bc95-b6627a0f0762', N'2eab62fb-8898-4f7c-9c99-4ce43bd88996', N'cWYEQTJZR1eKlUd-W3qzeD:APA91bGhhq-6Ub7p1yIV1wFh6VRXCLXeSRZOC-sJga-dBqXxDZC_SaxWm57AMTB1du-NPYlpKxUIPpM2IvvlKKwA15Si8LttpxEkTd_gNKJtnnpH5QzVXuzTK74wh4XXSLcibz9f-IJo', CAST(N'2023-04-04T19:08:39.260' AS DateTime))
INSERT [dbo].[DeviceToken] ([Id], [AccountId], [Token], [CreateAt]) VALUES (N'cf21de4c-e85a-4481-b253-cfa778e5ccdd', N'8ad9b7c4-290b-484d-9217-6f5b9b736bd3', N'dOyv4Z7ISeqRlglEzoieEL:APA91bGHNSdSi4k5EsIjGlnZoWcXApQdUhTe259IWK9BAJXLufXWd59fRSXuKSppUGrqF61WASj0TjX3rUmIUAXBgEBRNK9mlSmAoBLATsl2DGLtV2M7rw5pF6DKqqbYWsO8A2HV3CDk', CAST(N'2023-04-04T19:03:13.300' AS DateTime))
GO