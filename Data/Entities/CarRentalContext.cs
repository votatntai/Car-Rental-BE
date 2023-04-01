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
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC0742244F27");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Username, "UQ__Account__536C85E440C2A65B").IsUnique();

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
            entity.HasKey(e => e.Id).HasName("PK__Addition__3214EC0703E53A2E");

            entity.ToTable("AdditionalCharge");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Calendar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Calendar__3214EC07D6DF92AF");

            entity.ToTable("Calendar");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.EndTime).HasDefaultValueSql("('22:00:00')");
            entity.Property(e => e.StartTime).HasDefaultValueSql("('06:00:00')");
            entity.Property(e => e.Weekday).HasMaxLength(256);
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Car__3214EC07D67DB3AC");

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
                .HasConstraintName("FK__Car__AdditionalC__6754599E");

            entity.HasOne(d => d.CarOwner).WithMany(p => p.Cars)
                .HasForeignKey(d => d.CarOwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Car__CarOwnerId__693CA210");

            entity.HasOne(d => d.Driver).WithMany(p => p.Cars)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("FK__Car__DriverId__68487DD7");

            entity.HasOne(d => d.Location).WithMany(p => p.Cars)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Car__LocationId__66603565");

            entity.HasOne(d => d.Model).WithMany(p => p.Cars)
                .HasForeignKey(d => d.ModelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Car__ModelId__656C112C");

            entity.HasOne(d => d.Showroom).WithMany(p => p.Cars)
                .HasForeignKey(d => d.ShowroomId)
                .HasConstraintName("FK__Car__ShowroomId__6A30C649");
        });

        modelBuilder.Entity<CarCalendar>(entity =>
        {
            entity.HasKey(e => new { e.CalendarId, e.CarId }).HasName("PK__CarCalen__A545C70F0F59BDD3");

            entity.ToTable("CarCalendar");

            entity.HasOne(d => d.Calendar).WithMany(p => p.CarCalendars)
                .HasForeignKey(d => d.CalendarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarCalend__Calen__07C12930");

            entity.HasOne(d => d.Car).WithMany(p => p.CarCalendars)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarCalend__CarId__08B54D69");
        });

        modelBuilder.Entity<CarFeature>(entity =>
        {
            entity.HasKey(e => new { e.CarId, e.FeatureId }).HasName("PK__CarFeatu__E0820492FA730AE2");

            entity.ToTable("CarFeature");

            entity.HasOne(d => d.Car).WithMany(p => p.CarFeatures)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarFeatur__CarId__74AE54BC");

            entity.HasOne(d => d.Feature).WithMany(p => p.CarFeatures)
                .HasForeignKey(d => d.FeatureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarFeatur__Featu__75A278F5");
        });

        modelBuilder.Entity<CarOwner>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__CarOwner__349DA5A65185A6B2");

            entity.ToTable("CarOwner");

            entity.HasIndex(e => e.Phone, "UQ__CarOwner__5C7E359EBF4F1B5B").IsUnique();

            entity.HasIndex(e => e.WalletId, "UQ__CarOwner__84D4F90F5EDB707A").IsUnique();

            entity.Property(e => e.AccountId).ValueGeneratedNever();
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
                .HasConstraintName("FK__CarOwner__Accoun__46E78A0C");

            entity.HasOne(d => d.Wallet).WithOne(p => p.CarOwner)
                .HasForeignKey<CarOwner>(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarOwner__Wallet__47DBAE45");
        });

        modelBuilder.Entity<CarRegistration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CarRegis__3214EC07205A88EB");

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

            entity.HasOne(d => d.AdditionalCharge).WithMany(p => p.CarRegistrations)
                .HasForeignKey(d => d.AdditionalChargeId)
                .HasConstraintName("FK__CarRegist__Addit__00200768");

            entity.HasOne(d => d.CarOwner).WithMany(p => p.CarRegistrations)
                .HasForeignKey(d => d.CarOwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarRegist__CarOw__7F2BE32F");
        });

        modelBuilder.Entity<CarRegistrationCalendar>(entity =>
        {
            entity.HasKey(e => new { e.CalendarId, e.CarRegistrationId }).HasName("PK__CarRegis__FF9A9A472BDBCE12");

            entity.ToTable("CarRegistrationCalendar");

            entity.HasOne(d => d.Calendar).WithMany(p => p.CarRegistrationCalendars)
                .HasForeignKey(d => d.CalendarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarRegist__Calen__0B91BA14");

            entity.HasOne(d => d.CarRegistration).WithMany(p => p.CarRegistrationCalendars)
                .HasForeignKey(d => d.CarRegistrationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarRegist__CarRe__0C85DE4D");
        });

        modelBuilder.Entity<CarType>(entity =>
        {
            entity.HasKey(e => new { e.CarId, e.TypeId }).HasName("PK__CarType__2DB6C415D1086BF6");

            entity.ToTable("CarType");

            entity.HasOne(d => d.Car).WithMany(p => p.CarTypes)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarType__CarId__7A672E12");

            entity.HasOne(d => d.Type).WithMany(p => p.CarTypes)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarType__TypeId__7B5B524B");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Customer__349DA5A612D014F8");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.Phone, "UQ__Customer__5C7E359EC07CF63E").IsUnique();

            entity.HasIndex(e => e.WalletId, "UQ__Customer__84D4F90F1BF1226A").IsUnique();

            entity.Property(e => e.AccountId).ValueGeneratedNever();
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
                .HasConstraintName("FK__Customer__Accoun__412EB0B6");

            entity.HasOne(d => d.Wallet).WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Customer__Wallet__4222D4EF");
        });

        modelBuilder.Entity<DeviceToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DeviceTo__3214EC0770D98687");

            entity.ToTable("DeviceToken");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.DeviceTokens)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DeviceTok__Accou__571DF1D5");
        });

        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Driver__349DA5A626F4BAEB");

            entity.ToTable("Driver");

            entity.HasIndex(e => e.Phone, "UQ__Driver__5C7E359E94D591F5").IsUnique();

            entity.HasIndex(e => e.WalletId, "UQ__Driver__84D4F90F3D61EAFD").IsUnique();

            entity.Property(e => e.AccountId).ValueGeneratedNever();
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
                .HasConstraintName("FK__Driver__AccountI__4CA06362");

            entity.HasOne(d => d.Location).WithMany(p => p.Drivers)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Driver__Location__4E88ABD4");

            entity.HasOne(d => d.Wallet).WithOne(p => p.Driver)
                .HasForeignKey<Driver>(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Driver__WalletId__4D94879B");
        });

        modelBuilder.Entity<DriverCalendar>(entity =>
        {
            entity.HasKey(e => new { e.CalendarId, e.DriverId, e.CarId }).HasName("PK__DriverCa__23BC78A946FCF8EF");

            entity.ToTable("DriverCalendar");

            entity.HasOne(d => d.Calendar).WithMany(p => p.DriverCalendars)
                .HasForeignKey(d => d.CalendarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DriverCal__Calen__0F624AF8");

            entity.HasOne(d => d.Car).WithMany(p => p.DriverCalendars)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DriverCal__CarId__10566F31");

            entity.HasOne(d => d.Driver).WithMany(p => p.DriverCalendars)
                .HasForeignKey(d => d.DriverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DriverCal__Drive__114A936A");
        });

        modelBuilder.Entity<Feature>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feature__3214EC07C79FAD62");

            entity.ToTable("Feature");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<FeedBack>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FeedBack__3214EC075C0F0E5C");

            entity.ToTable("FeedBack");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Car).WithMany(p => p.FeedBacks)
                .HasForeignKey(d => d.CarId)
                .HasConstraintName("FK__FeedBack__CarId__30C33EC3");

            entity.HasOne(d => d.Customer).WithMany(p => p.FeedBacks)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FeedBack__Custom__2FCF1A8A");

            entity.HasOne(d => d.Driver).WithMany(p => p.FeedBacks)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("FK__FeedBack__Driver__31B762FC");

            entity.HasOne(d => d.Order).WithMany(p => p.FeedBacks)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FeedBack__OrderI__2EDAF651");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Image__3214EC07707CC97C");

            entity.ToTable("Image");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Type).HasMaxLength(256);
            entity.Property(e => e.Url).HasMaxLength(256);

            entity.HasOne(d => d.Car).WithMany(p => p.Images)
                .HasForeignKey(d => d.CarId)
                .HasConstraintName("FK__Image__CarId__367C1819");

            entity.HasOne(d => d.CarRegistration).WithMany(p => p.Images)
                .HasForeignKey(d => d.CarRegistrationId)
                .HasConstraintName("FK__Image__CarRegist__37703C52");

            entity.HasOne(d => d.Showroom).WithMany(p => p.Images)
                .HasForeignKey(d => d.ShowroomId)
                .HasConstraintName("FK__Image__ShowroomI__3587F3E0");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC078F1E77F3");

            entity.ToTable("Location");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Model>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Model__3214EC07BF5E31AB");

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
                .HasConstraintName("FK__Model__Productio__5FB337D6");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC07242C7FA4");

            entity.ToTable("Notification");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Link).HasMaxLength(256);
            entity.Property(e => e.Title).HasMaxLength(256);
            entity.Property(e => e.Type).HasMaxLength(256);

            entity.HasOne(d => d.Account).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__Accou__14270015");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC075C440E07");

            entity.ToTable("Order");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(256);

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__CustomerI__22751F6C");

            entity.HasOne(d => d.Promotion).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PromotionId)
                .HasConstraintName("FK__Order__Promotion__236943A5");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__3214EC07E3C21728");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DeliveryTime).HasColumnType("datetime");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.PickUpTime).HasColumnType("datetime");
            entity.Property(e => e.StartTime).HasColumnType("datetime");

            entity.HasOne(d => d.Car).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.CarId)
                .HasConstraintName("FK__OrderDeta__CarId__29221CFB");

            entity.HasOne(d => d.DeliveryLocation).WithMany(p => p.OrderDetailDeliveryLocations)
                .HasForeignKey(d => d.DeliveryLocationId)
                .HasConstraintName("FK__OrderDeta__Deliv__2A164134");

            entity.HasOne(d => d.Driver).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("FK__OrderDeta__Drive__2BFE89A6");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__Order__282DF8C2");

            entity.HasOne(d => d.PickUpLocation).WithMany(p => p.OrderDetailPickUpLocations)
                .HasForeignKey(d => d.PickUpLocationId)
                .HasConstraintName("FK__OrderDeta__PickU__2B0A656D");
        });

        modelBuilder.Entity<ProductionCompany>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producti__3214EC07BB4046F0");

            entity.ToTable("ProductionCompany");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Promotio__3214EC0707E18A1A");

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
            entity.HasKey(e => e.Id).HasName("PK__Showroom__3214EC07736F461C");

            entity.ToTable("Showroom");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(256);

            entity.HasOne(d => d.Location).WithMany(p => p.Showrooms)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Showroom__Locati__5AEE82B9");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC0768BDAEF4");

            entity.ToTable("Transaction");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(256);
            entity.Property(e => e.Type).HasMaxLength(256);

            entity.HasOne(d => d.CarOwner).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.CarOwnerId)
                .HasConstraintName("FK__Transacti__CarOw__1BC821DD");

            entity.HasOne(d => d.Customer).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Transacti__Custo__1AD3FDA4");

            entity.HasOne(d => d.Driver).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("FK__Transacti__Drive__19DFD96B");

            entity.HasOne(d => d.User).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Transacti__UserI__18EBB532");
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Type__3214EC07B7B85CD5");

            entity.ToTable("Type");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__User__349DA5A6B4E5D52A");

            entity.ToTable("User");

            entity.HasIndex(e => e.Phone, "UQ__User__5C7E359E5AE74A8D").IsUnique();

            entity.HasIndex(e => e.WalletId, "UQ__User__84D4F90FB7F38903").IsUnique();

            entity.Property(e => e.AccountId).ValueGeneratedNever();
            entity.Property(e => e.Gender).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.Phone)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Role).HasMaxLength(256);

            entity.HasOne(d => d.Account).WithOne(p => p.User)
                .HasForeignKey<User>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__AccountId__534D60F1");

            entity.HasOne(d => d.Wallet).WithOne(p => p.User)
                .HasForeignKey<User>(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__WalletId__5441852A");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Wallet__3214EC07A8A7FBD9");

            entity.ToTable("Wallet");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Status).HasMaxLength(256);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
