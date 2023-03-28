using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

public partial class CarRentalContext : DbContext
{
    public CarRentalContext()
    {
    }

    public CarRentalContext(DbContextOptions<CarRentalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AdditionalCharge> AdditionalCharges { get; set; }

    public virtual DbSet<Calendar> Calendars { get; set; }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<CarCalendar> CarCalendars { get; set; }

    public virtual DbSet<CarFeature> CarFeatures { get; set; }

    public virtual DbSet<CarOwner> CarOwners { get; set; }

    public virtual DbSet<CarRegistration> CarRegistrations { get; set; }

    public virtual DbSet<CarRegistrationCalendar> CarRegistrationCalendars { get; set; }

    public virtual DbSet<CarType> CarTypes { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DeviceToken> DeviceTokens { get; set; }

    public virtual DbSet<Driver> Drivers { get; set; }

    public virtual DbSet<DriverCalendar> DriverCalendars { get; set; }

    public virtual DbSet<Feature> Features { get; set; }

    public virtual DbSet<FeedBack> FeedBacks { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Model> Models { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<ProductionCompany> ProductionCompanies { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<Showroom> Showrooms { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Type> Types { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC073ADE56F0");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Username, "UQ__Account__536C85E44E006733").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Password)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(256)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AdditionalCharge>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Addition__3214EC07C045283F");

            entity.ToTable("AdditionalCharge");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Calendar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Calendar__3214EC075A122147");

            entity.ToTable("Calendar");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.EndTime).HasDefaultValueSql("('22:00:00')");
            entity.Property(e => e.StartTime).HasDefaultValueSql("('06:00:00')");
            entity.Property(e => e.Weekday).HasMaxLength(256);
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Car__3214EC0779227C7A");

            entity.ToTable("Car");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LicensePlate)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.ReceiveEndTime).HasDefaultValueSql("('22:00:00')");
            entity.Property(e => e.ReceiveStartTime).HasDefaultValueSql("('06:00:00')");
            entity.Property(e => e.ReturnEndTime).HasDefaultValueSql("('22:00:00')");
            entity.Property(e => e.ReturnStartTime).HasDefaultValueSql("('06:00:00')");
            entity.Property(e => e.Status).HasMaxLength(256);

            entity.HasOne(d => d.AdditionalCharge).WithMany(p => p.Cars)
                .HasForeignKey(d => d.AdditionalChargeId)
                .HasConstraintName("FK__Car__AdditionalC__6B24EA82");

            entity.HasOne(d => d.CarOwner).WithMany(p => p.Cars)
                .HasForeignKey(d => d.CarOwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Car__CarOwnerId__6D0D32F4");

            entity.HasOne(d => d.Driver).WithMany(p => p.Cars)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("FK__Car__DriverId__6C190EBB");

            entity.HasOne(d => d.Location).WithMany(p => p.Cars)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Car__LocationId__6A30C649");

            entity.HasOne(d => d.Model).WithMany(p => p.Cars)
                .HasForeignKey(d => d.ModelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Car__ModelId__693CA210");

            entity.HasOne(d => d.Showroom).WithMany(p => p.Cars)
                .HasForeignKey(d => d.ShowroomId)
                .HasConstraintName("FK__Car__ShowroomId__6E01572D");
        });

        modelBuilder.Entity<CarCalendar>(entity =>
        {
            entity.HasKey(e => new { e.CalendarId, e.CarId }).HasName("PK__CarCalen__A545C70F3F67AE07");

            entity.ToTable("CarCalendar");

            entity.HasOne(d => d.Calendar).WithMany(p => p.CarCalendars)
                .HasForeignKey(d => d.CalendarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarCalend__Calen__08B54D69");

            entity.HasOne(d => d.Car).WithMany(p => p.CarCalendars)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarCalend__CarId__09A971A2");
        });

        modelBuilder.Entity<CarFeature>(entity =>
        {
            entity.HasKey(e => new { e.CarId, e.FeatureId }).HasName("PK__CarFeatu__E08204925A2AFEEC");

            entity.ToTable("CarFeature");

            entity.HasOne(d => d.Car).WithMany(p => p.CarFeatures)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarFeatur__CarId__778AC167");

            entity.HasOne(d => d.Feature).WithMany(p => p.CarFeatures)
                .HasForeignKey(d => d.FeatureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarFeatur__Featu__787EE5A0");
        });

        modelBuilder.Entity<CarOwner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CarOwner__3214EC07D04DFF55");

            entity.ToTable("CarOwner");

            entity.HasIndex(e => e.AccountId, "UQ__CarOwner__349DA5A7E23DC9CA").IsUnique();

            entity.HasIndex(e => e.Phone, "UQ__CarOwner__5C7E359ED1EA144B").IsUnique();

            entity.HasIndex(e => e.WalletId, "UQ__CarOwner__84D4F90F9F331876").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(256);
            entity.Property(e => e.BankAccountNumber).HasMaxLength(256);
            entity.Property(e => e.BankName).HasMaxLength(256);
            entity.Property(e => e.Gender).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.Phone)
                .HasMaxLength(256)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithOne(p => p.CarOwner)
                .HasForeignKey<CarOwner>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarOwner__Accoun__48CFD27E");

            entity.HasOne(d => d.Wallet).WithOne(p => p.CarOwner)
                .HasForeignKey<CarOwner>(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarOwner__Wallet__49C3F6B7");
        });

        modelBuilder.Entity<CarRegistration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CarRegis__3214EC07B7A6F4F3");

            entity.ToTable("CarRegistration");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FuelConsumption).HasMaxLength(256);
            entity.Property(e => e.FuelType).HasMaxLength(256);
            entity.Property(e => e.LicensePlate)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Location).HasMaxLength(256);
            entity.Property(e => e.Model)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.ProductionCompany).HasMaxLength(256);
            entity.Property(e => e.TransmissionType).HasMaxLength(256);

            entity.HasOne(d => d.CarOwner).WithMany(p => p.CarRegistrations)
                .HasForeignKey(d => d.CarOwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarRegist__CarOw__02084FDA");
        });

        modelBuilder.Entity<CarRegistrationCalendar>(entity =>
        {
            entity.HasKey(e => new { e.CalendarId, e.CarRegistrationId }).HasName("PK__CarRegis__FF9A9A47EB579F33");

            entity.ToTable("CarRegistrationCalendar");

            entity.HasOne(d => d.Calendar).WithMany(p => p.CarRegistrationCalendars)
                .HasForeignKey(d => d.CalendarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarRegist__Calen__0C85DE4D");

            entity.HasOne(d => d.CarRegistration).WithMany(p => p.CarRegistrationCalendars)
                .HasForeignKey(d => d.CarRegistrationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarRegist__CarRe__0D7A0286");
        });

        modelBuilder.Entity<CarType>(entity =>
        {
            entity.HasKey(e => new { e.CarId, e.TypeId }).HasName("PK__CarType__2DB6C41587C294A5");

            entity.ToTable("CarType");

            entity.HasOne(d => d.Car).WithMany(p => p.CarTypes)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarType__CarId__7D439ABD");

            entity.HasOne(d => d.Type).WithMany(p => p.CarTypes)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarType__TypeId__7E37BEF6");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC071D21DB19");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.AccountId, "UQ__Customer__349DA5A785E2FFF7").IsUnique();

            entity.HasIndex(e => e.Phone, "UQ__Customer__5C7E359E7917DF67").IsUnique();

            entity.HasIndex(e => e.WalletId, "UQ__Customer__84D4F90FD4EBAD5B").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(256);
            entity.Property(e => e.BankAccountNumber).HasMaxLength(256);
            entity.Property(e => e.BankName).HasMaxLength(256);
            entity.Property(e => e.Gender).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.Phone)
                .HasMaxLength(256)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Customer__Accoun__4222D4EF");

            entity.HasOne(d => d.Wallet).WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Customer__Wallet__4316F928");
        });

        modelBuilder.Entity<DeviceToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DeviceTo__3214EC071ABEF180");

            entity.ToTable("DeviceToken");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.DeviceTokens)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DeviceTok__Accou__5AEE82B9");
        });

        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Driver__3214EC07C1BB1EBF");

            entity.ToTable("Driver");

            entity.HasIndex(e => e.AccountId, "UQ__Driver__349DA5A796056A1E").IsUnique();

            entity.HasIndex(e => e.Phone, "UQ__Driver__5C7E359EB2B49973").IsUnique();

            entity.HasIndex(e => e.WalletId, "UQ__Driver__84D4F90F5E1B54EA").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(256);
            entity.Property(e => e.BankAccountNumber).HasMaxLength(256);
            entity.Property(e => e.BankName).HasMaxLength(256);
            entity.Property(e => e.Gender).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.Phone)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasMaxLength(256);

            entity.HasOne(d => d.Account).WithOne(p => p.Driver)
                .HasForeignKey<Driver>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Driver__AccountI__4F7CD00D");

            entity.HasOne(d => d.Location).WithMany(p => p.Drivers)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Driver__Location__5165187F");

            entity.HasOne(d => d.Wallet).WithOne(p => p.Driver)
                .HasForeignKey<Driver>(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Driver__WalletId__5070F446");
        });

        modelBuilder.Entity<DriverCalendar>(entity =>
        {
            entity.HasKey(e => new { e.CalendarId, e.DriverId, e.CarId }).HasName("PK__DriverCa__23BC78A90AF56546");

            entity.ToTable("DriverCalendar");

            entity.HasOne(d => d.Calendar).WithMany(p => p.DriverCalendars)
                .HasForeignKey(d => d.CalendarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DriverCal__Calen__10566F31");

            entity.HasOne(d => d.Car).WithMany(p => p.DriverCalendars)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DriverCal__CarId__114A936A");

            entity.HasOne(d => d.Driver).WithMany(p => p.DriverCalendars)
                .HasForeignKey(d => d.DriverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DriverCal__Drive__123EB7A3");
        });

        modelBuilder.Entity<Feature>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feature__3214EC07350BADA6");

            entity.ToTable("Feature");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<FeedBack>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FeedBack__3214EC07215BFA61");

            entity.ToTable("FeedBack");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Car).WithMany(p => p.FeedBacks)
                .HasForeignKey(d => d.CarId)
                .HasConstraintName("FK__FeedBack__CarId__31B762FC");

            entity.HasOne(d => d.Customer).WithMany(p => p.FeedBacks)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FeedBack__Custom__30C33EC3");

            entity.HasOne(d => d.Driver).WithMany(p => p.FeedBacks)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("FK__FeedBack__Driver__32AB8735");

            entity.HasOne(d => d.Order).WithMany(p => p.FeedBacks)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FeedBack__OrderI__2FCF1A8A");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Image__3214EC071BB07591");

            entity.ToTable("Image");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Type).HasMaxLength(256);
            entity.Property(e => e.Url).HasMaxLength(256);

            entity.HasOne(d => d.Car).WithMany(p => p.Images)
                .HasForeignKey(d => d.CarId)
                .HasConstraintName("FK__Image__CarId__37703C52");

            entity.HasOne(d => d.CarRegistration).WithMany(p => p.Images)
                .HasForeignKey(d => d.CarRegistrationId)
                .HasConstraintName("FK__Image__CarRegist__3864608B");

            entity.HasOne(d => d.Showroom).WithMany(p => p.Images)
                .HasForeignKey(d => d.ShowroomId)
                .HasConstraintName("FK__Image__ShowroomI__367C1819");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC074A2A99A0");

            entity.ToTable("Location");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Model>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Model__3214EC07334479FA");

            entity.ToTable("Model");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Chassis).HasMaxLength(256);
            entity.Property(e => e.FuelConsumption).HasMaxLength(256);
            entity.Property(e => e.FuelType).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.TransmissionType).HasMaxLength(256);

            entity.HasOne(d => d.ProductionCompany).WithMany(p => p.Models)
                .HasForeignKey(d => d.ProductionCompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Model__Productio__6383C8BA");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC0783E858CC");

            entity.ToTable("Notification");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Link).HasMaxLength(256);
            entity.Property(e => e.Title).HasMaxLength(256);

            entity.HasOne(d => d.Account).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__Accou__151B244E");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC0715AF05C9");

            entity.ToTable("Order");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RentalTime).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(256);

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__CustomerI__236943A5");

            entity.HasOne(d => d.Promotion).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PromotionId)
                .HasConstraintName("FK__Order__Promotion__245D67DE");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__3214EC0737DA98E7");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DeliveryTime).HasColumnType("datetime");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.PickUpTime).HasColumnType("datetime");
            entity.Property(e => e.StartTime).HasColumnType("datetime");

            entity.HasOne(d => d.Car).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.CarId)
                .HasConstraintName("FK__OrderDeta__CarId__3C34F16F");

            entity.HasOne(d => d.DeliveryLocation).WithMany(p => p.OrderDetailDeliveryLocations)
                .HasForeignKey(d => d.DeliveryLocationId)
                .HasConstraintName("FK__OrderDeta__Deliv__3D2915A8");

            entity.HasOne(d => d.Driver).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("FK__OrderDeta__Drive__3F115E1A");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__Order__3B40CD36");

            entity.HasOne(d => d.PickUpLocation).WithMany(p => p.OrderDetailPickUpLocations)
                .HasForeignKey(d => d.PickUpLocationId)
                .HasConstraintName("FK__OrderDeta__PickU__3E1D39E1");
        });

        modelBuilder.Entity<ProductionCompany>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producti__3214EC0789044E57");

            entity.ToTable("ProductionCompany");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Promotio__3214EC07B79C6388");

            entity.ToTable("Promotion");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExpiryAt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<Showroom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Showroom__3214EC07C4129EB5");

            entity.ToTable("Showroom");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(256);

            entity.HasOne(d => d.Location).WithMany(p => p.Showrooms)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Showroom__Locati__5EBF139D");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC07EEEB561F");

            entity.ToTable("Transaction");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(256);
            entity.Property(e => e.Type).HasMaxLength(256);

            entity.HasOne(d => d.CarOwner).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.CarOwnerId)
                .HasConstraintName("FK__Transacti__CarOw__1CBC4616");

            entity.HasOne(d => d.Customer).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Transacti__Custo__1BC821DD");

            entity.HasOne(d => d.Driver).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("FK__Transacti__Drive__1AD3FDA4");

            entity.HasOne(d => d.User).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Transacti__UserI__19DFD96B");
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Type__3214EC0790CE91B8");

            entity.ToTable("Type");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC077D16B015");

            entity.ToTable("User");

            entity.HasIndex(e => e.AccountId, "UQ__User__349DA5A700171D27").IsUnique();

            entity.HasIndex(e => e.Phone, "UQ__User__5C7E359E66E344F7").IsUnique();

            entity.HasIndex(e => e.WalletId, "UQ__User__84D4F90FAFCB90B0").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Gender).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.Phone)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Role).HasMaxLength(256);

            entity.HasOne(d => d.Account).WithOne(p => p.User)
                .HasForeignKey<User>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__AccountId__571DF1D5");

            entity.HasOne(d => d.Wallet).WithOne(p => p.User)
                .HasForeignKey<User>(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__WalletId__5812160E");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Wallet__3214EC07C80818E0");

            entity.ToTable("Wallet");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Status).HasMaxLength(256);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
