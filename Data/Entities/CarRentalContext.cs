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
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC07C590EA23");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Username, "UQ__Account__536C85E4EFC4FB29").IsUnique();

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
            entity.HasKey(e => e.Id).HasName("PK__Addition__3214EC07055B8A22");

            entity.ToTable("AdditionalCharge");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Calendar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Calendar__3214EC078EE62E36");

            entity.ToTable("Calendar");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.EndTime).HasDefaultValueSql("('22:00:00')");
            entity.Property(e => e.StartTime).HasDefaultValueSql("('06:00:00')");
            entity.Property(e => e.Weekday).HasMaxLength(256);
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Car__3214EC07E37F6A6B");

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
                .HasConstraintName("FK__Car__AdditionalC__0C85DE4D");

            entity.HasOne(d => d.CarOwner).WithMany(p => p.Cars)
                .HasForeignKey(d => d.CarOwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Car__CarOwnerId__0E6E26BF");

            entity.HasOne(d => d.Driver).WithMany(p => p.Cars)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("FK__Car__DriverId__0D7A0286");

            entity.HasOne(d => d.Location).WithMany(p => p.Cars)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Car__LocationId__0B91BA14");

            entity.HasOne(d => d.Model).WithMany(p => p.Cars)
                .HasForeignKey(d => d.ModelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Car__ModelId__0A9D95DB");

            entity.HasOne(d => d.Showroom).WithMany(p => p.Cars)
                .HasForeignKey(d => d.ShowroomId)
                .HasConstraintName("FK__Car__ShowroomId__0F624AF8");
        });

        modelBuilder.Entity<CarCalendar>(entity =>
        {
            entity.HasKey(e => new { e.CalendarId, e.CarId }).HasName("PK__CarCalen__A545C70FCD194BA4");

            entity.ToTable("CarCalendar");

            entity.HasOne(d => d.Calendar).WithMany(p => p.CarCalendars)
                .HasForeignKey(d => d.CalendarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarCalend__Calen__2CF2ADDF");

            entity.HasOne(d => d.Car).WithMany(p => p.CarCalendars)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarCalend__CarId__2DE6D218");
        });

        modelBuilder.Entity<CarFeature>(entity =>
        {
            entity.HasKey(e => new { e.CarId, e.FeatureId }).HasName("PK__CarFeatu__E0820492770F39DD");

            entity.ToTable("CarFeature");

            entity.HasOne(d => d.Car).WithMany(p => p.CarFeatures)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarFeatur__CarId__19DFD96B");

            entity.HasOne(d => d.Feature).WithMany(p => p.CarFeatures)
                .HasForeignKey(d => d.FeatureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarFeatur__Featu__1AD3FDA4");
        });

        modelBuilder.Entity<CarOwner>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__CarOwner__349DA5A6FEF92219");

            entity.ToTable("CarOwner");

            entity.HasIndex(e => e.Phone, "UQ__CarOwner__5C7E359EBCE805A6").IsUnique();

            entity.HasIndex(e => e.WalletId, "UQ__CarOwner__84D4F90F19C45674").IsUnique();

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
                .HasConstraintName("FK__CarOwner__Accoun__6C190EBB");

            entity.HasOne(d => d.Wallet).WithOne(p => p.CarOwner)
                .HasForeignKey<CarOwner>(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarOwner__Wallet__6D0D32F4");
        });

        modelBuilder.Entity<CarRegistration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CarRegis__3214EC07CFADFFAA");

            entity.ToTable("CarRegistration");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Chassis)
                .HasMaxLength(256)
                .IsUnicode(false);
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
                .HasConstraintName("FK__CarRegist__Addit__25518C17");

            entity.HasOne(d => d.CarOwner).WithMany(p => p.CarRegistrations)
                .HasForeignKey(d => d.CarOwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarRegist__CarOw__245D67DE");
        });

        modelBuilder.Entity<CarRegistrationCalendar>(entity =>
        {
            entity.HasKey(e => new { e.CalendarId, e.CarRegistrationId }).HasName("PK__CarRegis__FF9A9A475D0217D0");

            entity.ToTable("CarRegistrationCalendar");

            entity.HasOne(d => d.Calendar).WithMany(p => p.CarRegistrationCalendars)
                .HasForeignKey(d => d.CalendarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarRegist__Calen__30C33EC3");

            entity.HasOne(d => d.CarRegistration).WithMany(p => p.CarRegistrationCalendars)
                .HasForeignKey(d => d.CarRegistrationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarRegist__CarRe__31B762FC");
        });

        modelBuilder.Entity<CarType>(entity =>
        {
            entity.HasKey(e => new { e.CarId, e.TypeId }).HasName("PK__CarType__2DB6C415026FEA9B");

            entity.ToTable("CarType");

            entity.HasOne(d => d.Car).WithMany(p => p.CarTypes)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarType__CarId__1F98B2C1");

            entity.HasOne(d => d.Type).WithMany(p => p.CarTypes)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarType__TypeId__208CD6FA");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Customer__349DA5A6FA40E558");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.Phone, "UQ__Customer__5C7E359EC5F1538F").IsUnique();

            entity.HasIndex(e => e.WalletId, "UQ__Customer__84D4F90FB097B98D").IsUnique();

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
                .HasConstraintName("FK__Customer__Accoun__66603565");

            entity.HasOne(d => d.Wallet).WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Customer__Wallet__6754599E");
        });

        modelBuilder.Entity<DeviceToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DeviceTo__3214EC0751DF6858");

            entity.ToTable("DeviceToken");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.DeviceTokens)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DeviceTok__Accou__7C4F7684");
        });

        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Driver__349DA5A63C43C8EB");

            entity.ToTable("Driver");

            entity.HasIndex(e => e.Phone, "UQ__Driver__5C7E359ED95EC766").IsUnique();

            entity.HasIndex(e => e.WalletId, "UQ__Driver__84D4F90F052EEB8B").IsUnique();

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
                .HasConstraintName("FK__Driver__AccountI__71D1E811");

            entity.HasOne(d => d.Location).WithMany(p => p.Drivers)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Driver__Location__73BA3083");

            entity.HasOne(d => d.Wallet).WithOne(p => p.Driver)
                .HasForeignKey<Driver>(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Driver__WalletId__72C60C4A");
        });

        modelBuilder.Entity<DriverCalendar>(entity =>
        {
            entity.HasKey(e => new { e.CalendarId, e.DriverId, e.CarId }).HasName("PK__DriverCa__23BC78A9681B64E9");

            entity.ToTable("DriverCalendar");

            entity.HasOne(d => d.Calendar).WithMany(p => p.DriverCalendars)
                .HasForeignKey(d => d.CalendarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DriverCal__Calen__3493CFA7");

            entity.HasOne(d => d.Car).WithMany(p => p.DriverCalendars)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DriverCal__CarId__3587F3E0");

            entity.HasOne(d => d.Driver).WithMany(p => p.DriverCalendars)
                .HasForeignKey(d => d.DriverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DriverCal__Drive__367C1819");
        });

        modelBuilder.Entity<Feature>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feature__3214EC073D9F875F");

            entity.ToTable("Feature");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<FeedBack>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FeedBack__3214EC0747DB8BA7");

            entity.ToTable("FeedBack");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Car).WithMany(p => p.FeedBacks)
                .HasForeignKey(d => d.CarId)
                .HasConstraintName("FK__FeedBack__CarId__55F4C372");

            entity.HasOne(d => d.Customer).WithMany(p => p.FeedBacks)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FeedBack__Custom__55009F39");

            entity.HasOne(d => d.Driver).WithMany(p => p.FeedBacks)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("FK__FeedBack__Driver__56E8E7AB");

            entity.HasOne(d => d.Order).WithMany(p => p.FeedBacks)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FeedBack__OrderI__540C7B00");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Image__3214EC07E4B1EDC7");

            entity.ToTable("Image");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Type).HasMaxLength(256);
            entity.Property(e => e.Url).HasMaxLength(256);

            entity.HasOne(d => d.Car).WithMany(p => p.Images)
                .HasForeignKey(d => d.CarId)
                .HasConstraintName("FK__Image__CarId__607251E5");

            entity.HasOne(d => d.CarRegistration).WithMany(p => p.Images)
                .HasForeignKey(d => d.CarRegistrationId)
                .HasConstraintName("FK__Image__CarRegist__6166761E");

            entity.HasOne(d => d.Customer).WithMany(p => p.Images)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Image__CustomerI__625A9A57");

            entity.HasOne(d => d.Driver).WithMany(p => p.Images)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("FK__Image__DriverId__634EBE90");

            entity.HasOne(d => d.Showroom).WithMany(p => p.Images)
                .HasForeignKey(d => d.ShowroomId)
                .HasConstraintName("FK__Image__ShowroomI__5F7E2DAC");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC07E1F44F01");

            entity.ToTable("Location");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Model>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Model__3214EC0724CFE37D");

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
                .HasConstraintName("FK__Model__Productio__04E4BC85");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC0763BF99FE");

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
                .HasConstraintName("FK__Notificat__Accou__395884C4");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC0767A2F876");

            entity.ToTable("Order");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(256);

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__CustomerI__47A6A41B");

            entity.HasOne(d => d.Promotion).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PromotionId)
                .HasConstraintName("FK__Order__Promotion__489AC854");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__3214EC0706BC6770");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DeliveryTime).HasColumnType("datetime");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.PickUpTime).HasColumnType("datetime");
            entity.Property(e => e.StartTime).HasColumnType("datetime");

            entity.HasOne(d => d.Car).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.CarId)
                .HasConstraintName("FK__OrderDeta__CarId__4E53A1AA");

            entity.HasOne(d => d.DeliveryLocation).WithMany(p => p.OrderDetailDeliveryLocations)
                .HasForeignKey(d => d.DeliveryLocationId)
                .HasConstraintName("FK__OrderDeta__Deliv__4F47C5E3");

            entity.HasOne(d => d.Driver).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("FK__OrderDeta__Drive__51300E55");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__Order__4D5F7D71");

            entity.HasOne(d => d.PickUpLocation).WithMany(p => p.OrderDetailPickUpLocations)
                .HasForeignKey(d => d.PickUpLocationId)
                .HasConstraintName("FK__OrderDeta__PickU__503BEA1C");
        });

        modelBuilder.Entity<ProductionCompany>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producti__3214EC077CDCD19A");

            entity.ToTable("ProductionCompany");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Promotio__3214EC0780D2587C");

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
            entity.HasKey(e => e.Id).HasName("PK__Showroom__3214EC0716F051CA");

            entity.ToTable("Showroom");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(256);

            entity.HasOne(d => d.Location).WithMany(p => p.Showrooms)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Showroom__Locati__00200768");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC077BF7CACD");

            entity.ToTable("Transaction");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(256);
            entity.Property(e => e.Type).HasMaxLength(256);

            entity.HasOne(d => d.CarOwner).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.CarOwnerId)
                .HasConstraintName("FK__Transacti__CarOw__40F9A68C");

            entity.HasOne(d => d.Customer).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Transacti__Custo__40058253");

            entity.HasOne(d => d.Driver).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("FK__Transacti__Drive__3F115E1A");

            entity.HasOne(d => d.User).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Transacti__UserI__3E1D39E1");
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Type__3214EC0760FBED71");

            entity.ToTable("Type");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__User__349DA5A6FA688390");

            entity.ToTable("User");

            entity.HasIndex(e => e.Phone, "UQ__User__5C7E359E3619AB07").IsUnique();

            entity.HasIndex(e => e.WalletId, "UQ__User__84D4F90F65EBF50C").IsUnique();

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
                .HasConstraintName("FK__User__AccountId__787EE5A0");

            entity.HasOne(d => d.Wallet).WithOne(p => p.User)
                .HasForeignKey<User>(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__WalletId__797309D9");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Wallet__3214EC07EFB6262A");

            entity.ToTable("Wallet");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Status).HasMaxLength(256);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
