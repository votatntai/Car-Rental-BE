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
	Status bit not null default 1
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
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	Address nvarchar(256),
	Phone varchar(256) not null unique,
	Gender nvarchar(256) not null,
	AvatarUrl nvarchar(max),
	BankAccountNumber nvarchar(256),
	BankName nvarchar(256),
	AccountId uniqueidentifier foreign key references Account(Id) not null,
	WalletId uniqueidentifier foreign key references Wallet(Id) not null,
)
Go
Create Table CarOwner(
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	Address nvarchar(256),
	Phone varchar(256) not null unique,
	Gender nvarchar(256) not null,
	AvatarUrl nvarchar(max),
	BankAccountNumber nvarchar(256),
	BankName nvarchar(256),
	AccountId uniqueidentifier foreign key references Account(Id) not null,
	WalletId uniqueidentifier foreign key references Wallet(Id) not null,
)
Go
Create Table Driver(
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	Address nvarchar(256),
	Phone varchar(256) not null unique,
	Gender nvarchar(256) not null,
	AvatarUrl nvarchar(max),
	Star float,
	BankAccountNumber nvarchar(256),
	BankName nvarchar(256),
	AccountId uniqueidentifier foreign key references Account(Id) not null,
	WalletId uniqueidentifier foreign key references Wallet(Id) not null,
	LocationId uniqueidentifier foreign key references [Location](Id),
	Status nvarchar(256) not null
)
Go
Create Table [User](
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	Phone varchar(256) not null unique,
	Gender nvarchar(256) not null,
	AvatarUrl nvarchar(max),
	Role nvarchar(256) not null,
	AccountId uniqueidentifier foreign key references Account(Id) not null,
	WalletId uniqueidentifier foreign key references Wallet(Id) not null,
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
Create Table [Route](
	Id uniqueidentifier primary key,
	MaximumDistance int not null,
	DistanceSurcharge float not null,
	TimeSurcharge float not null,
)
Go
Create Table Car(
	Id uniqueidentifier primary key,
	Name nvarchar(256),
	LicensePlate varchar(256) not null,
	TransmissionType nvarchar(256) not null,
	FuelType nvarchar(256) not null,
	Seater int not null,
	Price float not null,
	FuelConsumption nvarchar(256) not null,
	YearOfManufacture int not null,
	CreateAt datetime not null,
	Description nvarchar(max),
	ProductionCompanyId uniqueidentifier foreign key references ProductionCompany(Id) not null,
	LocationId uniqueidentifier foreign key references [Location](Id),
	RouteId uniqueidentifier foreign key references [Route](Id),
	Rented int not null default 0,
	Star float,
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
Create Table Calendar(
	Id uniqueidentifier primary key,
	StartTime datetime not null,
	EndTime datetime not null,
	CarId uniqueidentifier foreign key references Car(Id),
	DriverId uniqueidentifier foreign key references Driver(Id),
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
	Location nvarchar(256) not null,
	Type nvarchar(256)not null,
	CreateAt datetime not null,
	Description nvarchar(max),
)
Go
Create Table [FeedBack](
	Id uniqueidentifier primary key,
	CustomerId uniqueidentifier foreign key references Customer(Id) not null,
	CarId uniqueidentifier foreign key references Car(Id),
	DriverId uniqueidentifier foreign key references Driver(Id),
	Star int not null,
	CreateAt datetime not null default getDate(),
	Content nvarchar(max)
)
Go
Create Table [Transaction](
	Id uniqueidentifier primary key,
	UserId uniqueidentifier foreign key references [User](Id),
	DriverId uniqueidentifier foreign key references Driver(Id),
	CarOwnerId uniqueidentifier foreign key references CarOwner(Id),
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
	CustomerId uniqueidentifier foreign key references Customer(Id) not null,
	RentalTime datetime not null,
	Amount float not null,
	PromotionId uniqueidentifier foreign key references Promotion(Id),
	IsPaid bit not null default 0,
	Status nvarchar(256) not null,
	Description nvarchar(max)
)
Go
Create Table OrderDetail(
	Id uniqueidentifier primary key,
	CarId uniqueidentifier foreign key references Car(Id),
	DeliveryLocationId uniqueidentifier foreign key references [Location](Id),
	PickUpLocationId uniqueidentifier foreign key references [Location](Id),
	DeliveryTime datetime not null,
	PickUpTime datetime not null,
	DriverId uniqueidentifier foreign key references Driver(Id),
)
Go
Create Table ExpensesIncurred(
	Id uniqueidentifier primary key,
	OrderId uniqueidentifier foreign key references [Order](Id) not null,
	Amount float not null,
	Description nvarchar(max)
)
Go
Create Table [Image](
	Id uniqueidentifier primary key,
	Url nvarchar(256) not null,
	Type nvarchar(256) not null,
	ShowroomId uniqueidentifier foreign key references Showroom(Id),
	CarId uniqueidentifier foreign key references Car(Id),
	CarRegistrationId uniqueidentifier foreign key references CarRegistration(Id),
	ExpensesIncurredId uniqueidentifier foreign key references ExpensesIncurred(Id),
)
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
INSERT [dbo].[Account] ([Id], [Username], [Password], [Status]) VALUES (N'e8d284cf-32fc-45e5-a921-9ef553c1750a', N'namque', N'Namque123', 1)
INSERT [dbo].[Account] ([Id], [Username], [Password], [Status]) VALUES (N'0cb3dee7-43d3-41b8-9eb4-bb970180c195', N'aaaa', N'qweqwe', 1)
INSERT [dbo].[Account] ([Id], [Username], [Password], [Status]) VALUES (N'846c7407-3215-42f8-aead-ccfa1b279b85', N'manager5', N'123123', 1)
GO
INSERT [dbo].[Wallet] ([Id], [Balance], [Status]) VALUES (N'b5aab94d-f4df-4a46-80a9-030ce93fa20a', 0, N'Active')
INSERT [dbo].[Wallet] ([Id], [Balance], [Status]) VALUES (N'7324a193-f7c1-4748-8ffc-0654b581b7e4', 0, N'Active')
INSERT [dbo].[Wallet] ([Id], [Balance], [Status]) VALUES (N'9cc08fcc-361c-44a8-9432-132ebfe67dbd', 0, N'Active')
INSERT [dbo].[Wallet] ([Id], [Balance], [Status]) VALUES (N'e55e2b45-4033-44f4-b721-448ceeb632ac', 1000000, N'Active')
INSERT [dbo].[Wallet] ([Id], [Balance], [Status]) VALUES (N'38be23d3-7cbf-484f-9171-4d598289e40a', 0, N'Active')
INSERT [dbo].[Wallet] ([Id], [Balance], [Status]) VALUES (N'e71e369e-6cc3-4fab-9195-6aa35177aad7', 0, N'Active')
INSERT [dbo].[Wallet] ([Id], [Balance], [Status]) VALUES (N'e072f378-7446-4629-a03e-abe0de29ca21', 0, N'Active')
INSERT [dbo].[Wallet] ([Id], [Balance], [Status]) VALUES (N'5c8ab370-01b3-436d-9329-ae424bf3b2e5', 0, N'Active')
INSERT [dbo].[Wallet] ([Id], [Balance], [Status]) VALUES (N'a39c7824-b958-4365-8336-aeb8838a7c88', 0, N'Active')
INSERT [dbo].[Wallet] ([Id], [Balance], [Status]) VALUES (N'612f814c-2387-432b-9ba2-bf4c1d4ffd2d', 0, N'Active')
INSERT [dbo].[Wallet] ([Id], [Balance], [Status]) VALUES (N'd212e634-16b1-4bfc-b617-e6fc27ed8763', 0, N'Active')
INSERT [dbo].[Wallet] ([Id], [Balance], [Status]) VALUES (N'74668d25-18f4-48d1-8f48-f6a3c4a8b3d5', 0, N'Active')
GO
INSERT [dbo].[Customer] ([Id], [Name], [Address], [Phone], [Gender], [AvatarUrl], [BankAccountNumber], [BankName], [AccountId], [WalletId]) VALUES (N'e9a0d281-b6a9-49f4-bd34-7634c900bc75', N'Customer', N'Customer Address', N'0348040506', N'Nam', NULL, NULL, NULL, N'8ad9b7c4-290b-484d-9217-6f5b9b736bd3', N'74668d25-18f4-48d1-8f48-f6a3c4a8b3d5')
GO
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'311b7110-020d-407a-8c4e-520dfa5d1c45', 10.838333781154026, 106.83131318182623)
INSERT [dbo].[Location] ([Id], [Longitude], [Latitude]) VALUES (N'52016854-5f2c-4168-9e5b-914645a52578', 10.84127505108961, 106.8103872633409)
GO
INSERT [dbo].[ProductionCompany] ([Id], [Name], [Description]) VALUES (N'1a91dd16-48ec-447f-ad9a-50b0d03c294d', N'Audi', NULL)
GO
INSERT [dbo].[Route] ([Id], [MaximumDistance], [DistanceSurcharge], [TimeSurcharge]) VALUES (N'62817ad2-b664-4f51-8b58-5bb19354e5f3', 0, 0, 0)
INSERT [dbo].[Route] ([Id], [MaximumDistance], [DistanceSurcharge], [TimeSurcharge]) VALUES (N'df1da6af-d530-4d24-bfce-8d610a13162b', 0, 0, 0)
GO
INSERT [dbo].[Car] ([Id], [Name], [LicensePlate], [TransmissionType], [FuelType], [Seater], [Price], [FuelConsumption], [YearOfManufacture], [CreateAt], [Description], [ProductionCompanyId], [LocationId], [RouteId], [Rented], [Star], [Status]) VALUES (N'a640ad49-f4b3-47b9-b690-5c644c6b18f4', N'S-Start Light', N'71S1-14533', N'string', N'string', 1, 2500000, N'string', 2023, CAST(N'2023-03-07T23:47:31.873' AS DateTime), N'string', N'1a91dd16-48ec-447f-ad9a-50b0d03c294d', N'52016854-5f2c-4168-9e5b-914645a52578', N'62817ad2-b664-4f51-8b58-5bb19354e5f3', 0, NULL, N'Idle')
INSERT [dbo].[Car] ([Id], [Name], [LicensePlate], [TransmissionType], [FuelType], [Seater], [Price], [FuelConsumption], [YearOfManufacture], [CreateAt], [Description], [ProductionCompanyId], [LocationId], [RouteId], [Rented], [Star], [Status]) VALUES (N'72ab5ee5-6591-4529-9d8f-e7ca75a436cb', N'S-Start Light S101', N'71S1-14533', N'string', N'string', 1, 2500000, N'string', 2023, CAST(N'2023-03-07T23:49:03.243' AS DateTime), N'string', N'1a91dd16-48ec-447f-ad9a-50b0d03c294d', N'311b7110-020d-407a-8c4e-520dfa5d1c45', N'df1da6af-d530-4d24-bfce-8d610a13162b', 0, NULL, N'Idle')
GO
INSERT [dbo].[CarRegistration] ([Id], [Name], [LicensePlate], [TransmissionType], [FuelType], [Seater], [Price], [FuelConsumption], [YearOfManufacture], [ProductionCompany], [Location], [Type], [CreateAt], [Description]) VALUES (N'80093235-0688-4f53-9888-f2abf0a84a66', N'Audi M3', N'71-B1-16567', N'Hộp số tự động', N'Xăng', 7, 50000, N'Thấp', 2013, N'Audi', N'15A đường Tân Hòa 2', N'Bán tải', CAST(N'2023-02-28T15:01:32.700' AS DateTime), N'string')
GO
INSERT [dbo].[Driver] ([Id], [Name], [Address], [Phone], [Gender], [AvatarUrl], [Star], [BankAccountNumber], [BankName], [AccountId], [WalletId], [LocationId], [Status]) VALUES (N'e88ac41c-cf83-4996-814f-4552311a8142', N'Driver', N'Driver address', N'0345667887', N'Nam', NULL, NULL, NULL, NULL, N'2eab62fb-8898-4f7c-9c99-4ce43bd88996', N'38be23d3-7cbf-484f-9171-4d598289e40a', NULL, N'Idle')
GO
INSERT [dbo].[CarOwner] ([Id], [Name], [Address], [Phone], [Gender], [AvatarUrl], [BankAccountNumber], [BankName], [AccountId], [WalletId]) VALUES (N'9cd0ad8b-040b-4e1d-af92-580191e70b85', N'Car Owner', N'Car owner address', N'0345464757', N'Nam', NULL, NULL, NULL, N'f6c89e0e-7261-45d1-ad58-23122b5567c8', N'b5aab94d-f4df-4a46-80a9-030ce93fa20a')
GO
INSERT [dbo].[User] ([Id], [Name], [Phone], [Gender], [AvatarUrl], [Role], [AccountId], [WalletId]) VALUES (N'efd56b2d-92d5-4088-b3c6-119293b69cbc', N'Administrator', N'0339040899', N'Nam', NULL, N'Admin', N'd13fb3f2-6240-4304-b272-35595262f2f3', N'e55e2b45-4033-44f4-b721-448ceeb632ac')
INSERT [dbo].[User] ([Id], [Name], [Phone], [Gender], [AvatarUrl], [Role], [AccountId], [WalletId]) VALUES (N'9a20601b-8a32-4765-8f78-60004b1d90e5', N'Trung', N'0346557558', N'Khác', NULL, N'Manager', N'28c1235a-3bdf-41ae-9c17-74162e7257b7', N'9cc08fcc-361c-44a8-9432-132ebfe67dbd')
INSERT [dbo].[User] ([Id], [Name], [Phone], [Gender], [AvatarUrl], [Role], [AccountId], [WalletId]) VALUES (N'4552956e-56a6-4696-a282-8ba4d08120f4', N'Nhi', N'0324566577', N'Nữ', NULL, N'Manager', N'ddd746c7-c70c-45f0-aaca-78a56a81c1bc', N'e71e369e-6cc3-4fab-9195-6aa35177aad7')
INSERT [dbo].[User] ([Id], [Name], [Phone], [Gender], [AvatarUrl], [Role], [AccountId], [WalletId]) VALUES (N'cf773ea0-afea-4b9d-a028-9605926aa98e', N'Hiền', N'0346775558', N'Nữ', NULL, N'Manager', N'0cb3dee7-43d3-41b8-9eb4-bb970180c195', N'5c8ab370-01b3-436d-9329-ae424bf3b2e5')
INSERT [dbo].[User] ([Id], [Name], [Phone], [Gender], [AvatarUrl], [Role], [AccountId], [WalletId]) VALUES (N'b8c3046e-5ec8-4a2d-809f-9ff875c95b38', N'Linh', N'0345667788', N'Nữ', NULL, N'Manager', N'846c7407-3215-42f8-aead-ccfa1b279b85', N'7324a193-f7c1-4748-8ffc-0654b581b7e4')
INSERT [dbo].[User] ([Id], [Name], [Phone], [Gender], [AvatarUrl], [Role], [AccountId], [WalletId]) VALUES (N'7c2a6219-b54f-4f44-9713-a3518041cb66', N'Nam', N'0344556677', N'Nam', NULL, N'Manager', N'e8d284cf-32fc-45e5-a921-9ef553c1750a', N'e072f378-7446-4629-a03e-abe0de29ca21')
INSERT [dbo].[User] ([Id], [Name], [Phone], [Gender], [AvatarUrl], [Role], [AccountId], [WalletId]) VALUES (N'ea087d60-d2e3-4320-b8c0-b5f83523ef6b', N'Hải Đăng', N'0533445577', N'Nam', NULL, N'Manager', N'd5fa40fd-c9cc-421f-bfb5-707b9f6139b2', N'a39c7824-b958-4365-8336-aeb8838a7c88')
INSERT [dbo].[User] ([Id], [Name], [Phone], [Gender], [AvatarUrl], [Role], [AccountId], [WalletId]) VALUES (N'8a144cc6-3cfe-417f-9636-ca85f3b73b36', N'Lộc', N'0348040895', N'Nam', NULL, N'Manager', N'c2a6059c-3f0f-4f15-9c84-7b074cd70835', N'612f814c-2387-432b-9ba2-bf4c1d4ffd2d')
INSERT [dbo].[User] ([Id], [Name], [Phone], [Gender], [AvatarUrl], [Role], [AccountId], [WalletId]) VALUES (N'a87677ec-c0df-4b40-a6ed-dd78904f3ab3', N'Manager', N'0348040899', N'Nam', NULL, N'Manager', N'9f8a6e2e-b79f-4baf-8677-65ec40152658', N'd212e634-16b1-4bfc-b617-e6fc27ed8763')
GO
