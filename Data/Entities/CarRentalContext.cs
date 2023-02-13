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

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<ProductionCompany> ProductionCompanies { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Type> Types { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC07E12B8B41");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Username, "UQ__Account__536C85E49E135CBC").IsUnique();

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
            entity.HasKey(e => e.Id).HasName("PK__Calendar__3214EC0743094CDA");

            entity.ToTable("Calendar");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.StartTime).HasColumnType("datetime");

            entity.HasOne(d => d.Car).WithMany(p => p.Calendars)
                .HasForeignKey(d => d.CarId)
                .HasConstraintName("FK__Calendar__CarId__6477ECF3");

            entity.HasOne(d => d.Driver).WithMany(p => p.Calendars)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("FK__Calendar__Driver__656C112C");
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Car__3214EC0775D04FEB");

            entity.ToTable("Car");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.FuelConsumption).HasMaxLength(256);
            entity.Property(e => e.FuelType).HasMaxLength(256);
            entity.Property(e => e.LicensePlate)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.TransmissionType).HasMaxLength(256);

            entity.HasOne(d => d.Location).WithMany(p => p.Cars)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Car__LocationId__5441852A");

            entity.HasOne(d => d.ProductionCompany).WithMany(p => p.Cars)
                .HasForeignKey(d => d.ProductionCompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Car__ProductionC__534D60F1");

            entity.HasOne(d => d.Route).WithMany(p => p.Cars)
                .HasForeignKey(d => d.RouteId)
                .HasConstraintName("FK__Car__RouteId__5535A963");
        });

        modelBuilder.Entity<CarFeature>(entity =>
        {
            entity.HasKey(e => new { e.CarId, e.FeatureId }).HasName("PK__CarFeatu__E0820492E88CCDDB");

            entity.ToTable("CarFeature");

            entity.HasOne(d => d.Car).WithMany(p => p.CarFeatures)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarFeatur__CarId__5AEE82B9");

            entity.HasOne(d => d.Feature).WithMany(p => p.CarFeatures)
                .HasForeignKey(d => d.FeatureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarFeatur__Featu__5BE2A6F2");
        });

        modelBuilder.Entity<CarOwner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CarOwner__3214EC07587AB099");

            entity.ToTable("CarOwner");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(256);
            entity.Property(e => e.BankAccountNumber).HasMaxLength(256);
            entity.Property(e => e.BankName).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.Phone)
                .HasMaxLength(256)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.CarOwners)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarOwner__Accoun__4222D4EF");

            entity.HasOne(d => d.Wallet).WithMany(p => p.CarOwners)
                .HasForeignKey(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarOwner__Wallet__4316F928");
        });

        modelBuilder.Entity<CarRegistration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CarRegis__3214EC07D717029C");

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
            entity.HasKey(e => new { e.CarId, e.TypeId }).HasName("PK__CarType__2DB6C4156968C2DD");

            entity.ToTable("CarType");

            entity.HasOne(d => d.Car).WithMany(p => p.CarTypes)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarType__CarId__60A75C0F");

            entity.HasOne(d => d.Type).WithMany(p => p.CarTypes)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarType__TypeId__619B8048");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC07B20E512C");

            entity.ToTable("Customer");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(256);
            entity.Property(e => e.BankAccountNumber).HasMaxLength(256);
            entity.Property(e => e.BankName).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.Phone)
                .HasMaxLength(256)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.Customers)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Customer__Accoun__3E52440B");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Customers)
                .HasForeignKey(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Customer__Wallet__3F466844");
        });

        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Driver__3214EC078BD14510");

            entity.ToTable("Driver");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(256);
            entity.Property(e => e.BankAccountNumber).HasMaxLength(256);
            entity.Property(e => e.BankName).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.Phone)
                .HasMaxLength(256)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.Drivers)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Driver__AccountI__45F365D3");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Drivers)
                .HasForeignKey(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Driver__WalletId__46E78A0C");
        });

        modelBuilder.Entity<ExpensesIncurred>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Expenses__3214EC07A2DD0791");

            entity.ToTable("ExpensesIncurred");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Order).WithMany(p => p.ExpensesIncurreds)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ExpensesI__Order__02FC7413");
        });

        modelBuilder.Entity<Feature>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feature__3214EC07665BCF86");

            entity.ToTable("Feature");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<FeedBack>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FeedBack__3214EC07498E13A7");

            entity.ToTable("FeedBack");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Car).WithMany(p => p.FeedBacks)
                .HasForeignKey(d => d.CarId)
                .HasConstraintName("FK__FeedBack__CarId__6B24EA82");

            entity.HasOne(d => d.Customer).WithMany(p => p.FeedBacks)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FeedBack__Custom__6A30C649");

            entity.HasOne(d => d.Driver).WithMany(p => p.FeedBacks)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("FK__FeedBack__Driver__6C190EBB");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Image__3214EC079D650157");

            entity.ToTable("Image");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Type).HasMaxLength(256);
            entity.Property(e => e.Url).HasMaxLength(256);

            entity.HasOne(d => d.Car).WithMany(p => p.Images)
                .HasForeignKey(d => d.CarId)
                .HasConstraintName("FK__Image__CarId__05D8E0BE");

            entity.HasOne(d => d.CarRegistration).WithMany(p => p.Images)
                .HasForeignKey(d => d.CarRegistrationId)
                .HasConstraintName("FK__Image__CarRegist__06CD04F7");

            entity.HasOne(d => d.ExpensesIncurred).WithMany(p => p.Images)
                .HasForeignKey(d => d.ExpensesIncurredId)
                .HasConstraintName("FK__Image__ExpensesI__07C12930");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC0774018740");

            entity.ToTable("Location");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Latitude)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Longitude)
                .HasMaxLength(256)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC07C4A14EE0");

            entity.ToTable("Order");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.RentalTime).HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__CustomerI__787EE5A0");

            entity.HasOne(d => d.Promotion).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PromotionId)
                .HasConstraintName("FK__Order__Promotion__797309D9");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__3214EC072EAFF51F");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DeliveryTime).HasColumnType("datetime");
            entity.Property(e => e.PickUpTime).HasColumnType("datetime");

            entity.HasOne(d => d.Car).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.CarId)
                .HasConstraintName("FK__OrderDeta__CarId__7D439ABD");

            entity.HasOne(d => d.DeliveryLocation).WithMany(p => p.OrderDetailDeliveryLocations)
                .HasForeignKey(d => d.DeliveryLocationId)
                .HasConstraintName("FK__OrderDeta__Deliv__7E37BEF6");

            entity.HasOne(d => d.Driver).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("FK__OrderDeta__Drive__00200768");

            entity.HasOne(d => d.PickUpLocation).WithMany(p => p.OrderDetailPickUpLocations)
                .HasForeignKey(d => d.PickUpLocationId)
                .HasConstraintName("FK__OrderDeta__PickU__7F2BE32F");
        });

        modelBuilder.Entity<ProductionCompany>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producti__3214EC071FDAD2FA");

            entity.ToTable("ProductionCompany");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Promotio__3214EC07733B1385");

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
            entity.HasKey(e => e.Id).HasName("PK__Route__3214EC07CB93D79C");

            entity.ToTable("Route");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC07247FAE29");

            entity.ToTable("Transaction");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(256);
            entity.Property(e => e.Type).HasMaxLength(256);

            entity.HasOne(d => d.CarOwner).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.CarOwnerId)
                .HasConstraintName("FK__Transacti__CarOw__71D1E811");

            entity.HasOne(d => d.Driver).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("FK__Transacti__Drive__70DDC3D8");

            entity.HasOne(d => d.User).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Transacti__UserI__6FE99F9F");
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Type__3214EC07BC986842");

            entity.ToTable("Type");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC071F930552");

            entity.ToTable("User");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.Phone)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Role).HasMaxLength(256);

            entity.HasOne(d => d.Account).WithMany(p => p.Users)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__AccountId__49C3F6B7");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Users)
                .HasForeignKey(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__WalletId__4AB81AF0");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Wallet__3214EC07B70B8937");

            entity.ToTable("Wallet");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Status).HasMaxLength(256);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
