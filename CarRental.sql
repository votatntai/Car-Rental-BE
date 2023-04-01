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
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'bfc4a613-fc5f-4258-b28a-015ca630a98d', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'29d84a42-9255-4909-8e2c-01f339704703', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'53fc103b-6709-44a2-9032-0a504702d32e', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'9afa2d11-b5c2-4988-8b80-0daedcf6bafd', 106.84312159786212, 10.848212845384804)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'0196c3a7-d330-4638-acb4-0f8425ac32b6', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'ccc8dc62-899c-47c2-ac69-11343a3bd0fb', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'7f061997-ebd2-4d7b-8fa2-153c47b7e72d', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'd4364a7e-7f84-4797-b9b1-16e355ced077', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'e51ef666-ca8b-4789-b3e1-1bb4b8b3202f', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'969d1e97-a7a7-4a6d-b362-1d56cd5afe6c', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'6d58ddd7-6cc6-43e0-ab3b-22a3daf1699e', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'bfaaa46f-52e2-444a-ad05-25d6971c601c', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'c638e506-07f5-45af-8aca-2ae9f2a00500', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'2e9909b5-27e3-4096-81e8-2f2f822fecb0', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'80339293-404f-4ab8-a95c-30c4614d1d3c', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'0ebc2757-c2ca-4f34-be04-325e254b7000', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'3b0f21a6-b9d4-40e3-bc7c-3bbc6da6496c', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'2a519e6a-2c44-4034-b132-3c60f31e1c69', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'a31e1be7-de22-4e41-9049-3d412e42d898', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'c08bf3ea-f204-48dc-b043-435a9852fb82', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'8cabd1ec-5a67-47b3-9ea8-459740eb48da', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'76b20fdc-bb02-4987-b68b-4f4f31573397', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'5958b4ae-97c2-4343-8a42-5425f8e7c174', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'676fe34b-b092-4f33-b5a5-554311151dbb', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'ae72a34a-0f49-448a-a783-55616d65d870', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'6e79f38c-f586-4878-9d87-5c88408bcde0', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'e487376a-7a81-4f9e-bb00-5f9d0088603c', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'24a6c020-5032-4cce-adc2-6215c20b03b4', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'3c8d6ae2-3e23-4f12-9f06-6383230fd759', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'50a9071b-91b7-44dd-8b43-642916e758bb', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'bc2aa8ac-ed48-4ceb-889b-6e703091b47f', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'4ada4796-61f7-44c6-b97f-71fc2de848fa', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'36b41efa-161d-4939-bcbe-737d532c056c', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'36e5b5ba-87b7-4d81-bea8-7af7b56076ee', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'd51f67ee-260b-4b0c-88ba-7b5b8b065cc8', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'14c420b0-18ab-41ce-b0ab-7d754ff6c91c', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'07332f75-a651-488d-8cd3-80fc7988884e', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'9a52d75d-6f9d-4aaf-9c64-81db57e6b8a9', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'd032f86c-0291-4540-927e-86878028fb70', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'96a5f913-8179-42ec-97fa-8aaf235e0c51', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'b159db0e-0cb5-41a3-8069-8fe2847347e2', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'e17e77e5-bbaa-4501-ad66-9002311b98f2', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'c0b756a8-a6da-4def-904c-947d138a053f', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'1f8bd6b0-6a4d-428d-a712-950815b6b746', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'1a35a04f-dc2c-431c-96d6-a54e37ae903d', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'f8bc25c6-af30-4758-b190-beaee523174b', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'2b697e8c-bafb-4a9c-b1f3-bf4c6a96f3f3', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'a9f36cd8-7b0f-4c33-8104-c5dcd4e49463', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'27a331a5-8dec-4f8b-ae22-c7e470d7bb7d', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'fcbecbe6-a53b-40bb-b7f5-cc1d897f6eea', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'7f4afa84-4365-42a4-884c-cf0e8a67699f', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'5169b33f-ffd7-4b74-aab3-d0cfe619e31d', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'a523984f-a9bb-4fae-bf10-d5241489241b', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'1f3c49dc-e9cc-41e7-b276-d687ab4e97d9', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'6cb1705c-d94c-401f-8bc1-d75d1bd09b02', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'7bb46c58-97f5-49c7-8fa4-d7842affa5e7', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'54a71bc2-711c-48dc-8eed-de4ef22b36fb', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'1568a664-a74e-4f65-ba87-de9b2b33fd1a', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'20a10241-bdfe-467d-8581-e11be6221373', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'68751161-cd78-403f-ae24-f30f33e7c0d6', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'1fdc95d8-14c4-42dc-9d55-f68d9be8e6a1', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'b9b494fd-6525-42ca-b6be-fc2f46ff357a', 0, 0)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'b613580b-7a7e-4082-832a-fdb8534c959a', 0, 0)
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
INSERT [dbo].[Car] ([Id], [Name], [LicensePlate], [Price], [CreateAt], [Description], [ModelId], [LocationId], [AdditionalChargeId], [DriverId], [CarOwnerId], [ShowroomId], [Rented], [ReceiveStartTime], [ReceiveEndTime], [ReturnStartTime], [ReturnEndTime], [Star], [Status]) VALUES (N'3087f92b-689e-49f8-ae67-c03f3db3b3c7', N'Ford Mustang
', N'29B-12345
', 100000, CAST(N'2023-04-01T09:17:33.657' AS DateTime), NULL, N'9b9505c0-6db3-4cfb-9ca4-97c0d5218dbb', N'9afa2d11-b5c2-4988-8b80-0daedcf6bafd', N'f99b6242-0b31-4790-a654-504b828bbc19', NULL, N'f6c89e0e-7261-45d1-ad58-23122b5567c8', NULL, 11, CAST(N'06:00:00' AS Time), CAST(N'22:00:00' AS Time), CAST(N'06:00:00' AS Time), CAST(N'22:00:00' AS Time), NULL, N'Working')
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
