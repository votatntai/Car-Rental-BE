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
Create Table Customer(
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	Address nvarchar(256),
	Phone varchar(256) not null,
	AvartarUrl nvarchar(max),
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
	Phone varchar(256) not null,
	AvartarUrl nvarchar(max),
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
	Phone varchar(256) not null,
	AvartarUrl nvarchar(max),
	Star float,
	BankAccountNumber nvarchar(256),
	BankName nvarchar(256),
	AccountId uniqueidentifier foreign key references Account(Id) not null,
	WalletId uniqueidentifier foreign key references Wallet(Id) not null,
)
Go
Create Table [User](
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	Phone varchar(256) not null,
	AvartarUrl nvarchar(max),
	Role nvarchar(256) not null,
	AccountId uniqueidentifier foreign key references Account(Id) not null,
	WalletId uniqueidentifier foreign key references Wallet(Id) not null,
)
Go
Create Table ProductionCompany(
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	Description nvarchar(max),
)
Go
Create Table [Location](
	Id uniqueidentifier primary key,
	Longitude varchar(256) not null,
	Latitude varchar(256) not null
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
	CarId uniqueidentifier foreign key references Car(Id),
	CarRegistrationId uniqueidentifier foreign key references CarRegistration(Id),
	ExpensesIncurredId uniqueidentifier foreign key references ExpensesIncurred(Id),
)
