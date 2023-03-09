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

    public virtual DbSet<Calendar> Calendars { get; set; }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<CarFeature> CarFeatures { get; set; }

    public virtual DbSet<CarOwner> CarOwners { get; set; }

    public virtual DbSet<CarRegistration> CarRegistrations { get; set; }

    public virtual DbSet<CarType> CarTypes { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Driver> Drivers { get; set; }

    public virtual DbSet<ExpensesIncurred> ExpensesIncurreds { get; set; }

    public virtual DbSet<Feature> Features { get; set; }

    public virtual DbSet<FeedBack> FeedBacks { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<ProductionCompany> ProductionCompanies { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<Showroom> Showrooms { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Type> Types { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC07F1EDADCA");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Username, "UQ__Account__536C85E4F80A2D98").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Password)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Username)
                .HasMaxLength(256)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Calendar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Calendar__3214EC07F5ACAEBD");

            entity.ToTable("Calendar");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.StartTime).HasColumnType("datetime");

            entity.HasOne(d => d.Car).WithMany(p => p.Calendars)
                .HasForeignKey(d => d.CarId)
                .HasConstraintName("FK__Calendar__CarId__6C190EBB");

            entity.HasOne(d => d.Driver).WithMany(p => p.Calendars)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("FK__Calendar__Driver__6D0D32F4");
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Car__3214EC0757C2A839");

            entity.ToTable("Car");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.FuelConsumption).HasMaxLength(256);
            entity.Property(e => e.FuelType).HasMaxLength(256);
            entity.Property(e => e.LicensePlate)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.Status).HasMaxLength(256);
            entity.Property(e => e.TransmissionType).HasMaxLength(256);

            entity.HasOne(d => d.Location).WithMany(p => p.Cars)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Car__LocationId__5BE2A6F2");

            entity.HasOne(d => d.ProductionCompany).WithMany(p => p.Cars)
                .HasForeignKey(d => d.ProductionCompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Car__ProductionC__5AEE82B9");

            entity.HasOne(d => d.Route).WithMany(p => p.Cars)
                .HasForeignKey(d => d.RouteId)
                .HasConstraintName("FK__Car__RouteId__5CD6CB2B");
        });

        modelBuilder.Entity<CarFeature>(entity =>
        {
            entity.HasKey(e => new { e.CarId, e.FeatureId }).HasName("PK__CarFeatu__E082049226CA1C15");

            entity.ToTable("CarFeature");

            entity.HasOne(d => d.Car).WithMany(p => p.CarFeatures)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarFeatur__CarId__628FA481");

            entity.HasOne(d => d.Feature).WithMany(p => p.CarFeatures)
                .HasForeignKey(d => d.FeatureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarFeatur__Featu__6383C8BA");
        });

        modelBuilder.Entity<CarOwner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CarOwner__3214EC070F82EDA9");

            entity.ToTable("CarOwner");

            entity.HasIndex(e => e.Phone, "UQ__CarOwner__5C7E359E35D17131").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(256);
            entity.Property(e => e.BankAccountNumber).HasMaxLength(256);
            entity.Property(e => e.BankName).HasMaxLength(256);
            entity.Property(e => e.Gender).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.Phone)
                .HasMaxLength(256)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.CarOwners)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarOwner__Accoun__45F365D3");

            entity.HasOne(d => d.Wallet).WithMany(p => p.CarOwners)
                .HasForeignKey(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarOwner__Wallet__46E78A0C");
        });

        modelBuilder.Entity<CarRegistration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CarRegis__3214EC078F1A5981");

            entity.ToTable("CarRegistration");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.FuelConsumption).HasMaxLength(256);
            entity.Property(e => e.FuelType).HasMaxLength(256);
            entity.Property(e => e.LicensePlate)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Location).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.ProductionCompany).HasMaxLength(256);
            entity.Property(e => e.TransmissionType).HasMaxLength(256);
            entity.Property(e => e.Type).HasMaxLength(256);
        });

        modelBuilder.Entity<CarType>(entity =>
        {
            entity.HasKey(e => new { e.CarId, e.TypeId }).HasName("PK__CarType__2DB6C4154B385A27");

            entity.ToTable("CarType");

            entity.HasOne(d => d.Car).WithMany(p => p.CarTypes)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarType__CarId__68487DD7");

            entity.HasOne(d => d.Type).WithMany(p => p.CarTypes)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarType__TypeId__693CA210");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC07008C5124");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.Phone, "UQ__Customer__5C7E359E355CC7F3").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(256);
            entity.Property(e => e.BankAccountNumber).HasMaxLength(256);
            entity.Property(e => e.BankName).HasMaxLength(256);
            entity.Property(e => e.Gender).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.Phone)
                .HasMaxLength(256)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.Customers)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Customer__Accoun__412EB0B6");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Customers)
                .HasForeignKey(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Customer__Wallet__4222D4EF");
        });

        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Driver__3214EC07EE67C133");

            entity.ToTable("Driver");

            entity.HasIndex(e => e.Phone, "UQ__Driver__5C7E359EE0EE1163").IsUnique();

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

            entity.HasOne(d => d.Account).WithMany(p => p.Drivers)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Driver__AccountI__4AB81AF0");

            entity.HasOne(d => d.Location).WithMany(p => p.Drivers)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Driver__Location__4CA06362");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Drivers)
                .HasForeignKey(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Driver__WalletId__4BAC3F29");
        });

        modelBuilder.Entity<ExpensesIncurred>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Expenses__3214EC072E1F31A2");

            entity.ToTable("ExpensesIncurred");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Order).WithMany(p => p.ExpensesIncurreds)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ExpensesI__Order__0F624AF8");
        });

        modelBuilder.Entity<Feature>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feature__3214EC077188E04E");

            entity.ToTable("Feature");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<FeedBack>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FeedBack__3214EC07C72CF64E");

            entity.ToTable("FeedBack");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Car).WithMany(p => p.FeedBacks)
                .HasForeignKey(d => d.CarId)
                .HasConstraintName("FK__FeedBack__CarId__778AC167");

            entity.HasOne(d => d.Customer).WithMany(p => p.FeedBacks)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FeedBack__Custom__76969D2E");

            entity.HasOne(d => d.Driver).WithMany(p => p.FeedBacks)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("FK__FeedBack__Driver__787EE5A0");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Image__3214EC07B806CDA9");

            entity.ToTable("Image");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Type).HasMaxLength(256);
            entity.Property(e => e.Url).HasMaxLength(256);

            entity.HasOne(d => d.Car).WithMany(p => p.Images)
                .HasForeignKey(d => d.CarId)
                .HasConstraintName("FK__Image__CarId__1332DBDC");

            entity.HasOne(d => d.CarRegistration).WithMany(p => p.Images)
                .HasForeignKey(d => d.CarRegistrationId)
                .HasConstraintName("FK__Image__CarRegist__14270015");

            entity.HasOne(d => d.ExpensesIncurred).WithMany(p => p.Images)
                .HasForeignKey(d => d.ExpensesIncurredId)
                .HasConstraintName("FK__Image__ExpensesI__151B244E");

            entity.HasOne(d => d.Showroom).WithMany(p => p.Images)
                .HasForeignKey(d => d.ShowroomId)
                .HasConstraintName("FK__Image__ShowroomI__123EB7A3");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC07841C6601");

            entity.ToTable("Location");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC07E76A6EA7");

            entity.ToTable("Notification");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__Accou__71D1E811");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC072A41F239");

            entity.ToTable("Order");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.RentalTime).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(256);

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__CustomerI__04E4BC85");

            entity.HasOne(d => d.Promotion).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PromotionId)
                .HasConstraintName("FK__Order__Promotion__05D8E0BE");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__3214EC074C6E11FA");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DeliveryTime).HasColumnType("datetime");
            entity.Property(e => e.PickUpTime).HasColumnType("datetime");

            entity.HasOne(d => d.Car).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.CarId)
                .HasConstraintName("FK__OrderDeta__CarId__09A971A2");

            entity.HasOne(d => d.DeliveryLocation).WithMany(p => p.OrderDetailDeliveryLocations)
                .HasForeignKey(d => d.DeliveryLocationId)
                .HasConstraintName("FK__OrderDeta__Deliv__0A9D95DB");

            entity.HasOne(d => d.Driver).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("FK__OrderDeta__Drive__0C85DE4D");

            entity.HasOne(d => d.PickUpLocation).WithMany(p => p.OrderDetailPickUpLocations)
                .HasForeignKey(d => d.PickUpLocationId)
                .HasConstraintName("FK__OrderDeta__PickU__0B91BA14");
        });

        modelBuilder.Entity<ProductionCompany>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producti__3214EC072945E0D2");

            entity.ToTable("ProductionCompany");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Promotio__3214EC07B1B27A1E");

            entity.ToTable("Promotion");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExpiryAt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Route__3214EC071D296E75");

            entity.ToTable("Route");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Showroom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Showroom__3214EC079C6F9D06");

            entity.ToTable("Showroom");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(256);

            entity.HasOne(d => d.Location).WithMany(p => p.Showrooms)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Showroom__Locati__5441852A");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC074377F059");

            entity.ToTable("Transaction");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(256);
            entity.Property(e => e.Type).HasMaxLength(256);

            entity.HasOne(d => d.CarOwner).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.CarOwnerId)
                .HasConstraintName("FK__Transacti__CarOw__7E37BEF6");

            entity.HasOne(d => d.Driver).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("FK__Transacti__Drive__7D439ABD");

            entity.HasOne(d => d.User).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Transacti__UserI__7C4F7684");
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Type__3214EC0748978043");

            entity.ToTable("Type");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07ED8CE55B");

            entity.ToTable("User");

            entity.HasIndex(e => e.Phone, "UQ__User__5C7E359E98EAA4F2").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Gender).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.Phone)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Role).HasMaxLength(256);

            entity.HasOne(d => d.Account).WithMany(p => p.Users)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__AccountId__5070F446");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Users)
                .HasForeignKey(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__WalletId__5165187F");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Wallet__3214EC07BF9A6A95");

            entity.ToTable("Wallet");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Status).HasMaxLength(256);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
