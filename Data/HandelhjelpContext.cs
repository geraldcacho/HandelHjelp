using HandleHjelp.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HandleHjelp.Data;

public partial class HandelhjelpContext : IdentityDbContext<ApplicationUser>
{
    public HandelhjelpContext()
    {
    }

    public HandelhjelpContext(DbContextOptions<HandelhjelpContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderProductType> OrderProductTypes { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<UserAddress> UserAddresses { get; set; }

    public virtual DbSet<UserVehicleType> UserVehicleTypes { get; set; }

    public virtual DbSet<VehicleType> VehicleTypes { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("Identity");
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable(name: "User");
        });
        modelBuilder.Entity<IdentityRole>(entity =>
        {
            entity.ToTable(name: "Role");
        });
        modelBuilder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.ToTable("UserRoles");
        });
        modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.ToTable("UserClaims");
        });
        modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.ToTable("UserLogins");
        });
        modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.ToTable("RoleClaims");
        });
        modelBuilder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.ToTable("UserTokens");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Code);

            entity.ToTable("Country", "hand");

            entity.Property(e => e.Code)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAFC7043873");

            entity.ToTable("Order", "hand");

            entity.Property(e => e.OrderId)
                .HasMaxLength(250)
                .HasColumnName("OrderID");
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CountryName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerId)
                .HasMaxLength(450)
                .HasColumnName("CustomerID");
            entity.Property(e => e.Latitude).HasColumnType("decimal(8, 6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Note)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PostalCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ReceivedOn).HasColumnType("datetime");
            entity.Property(e => e.StreetAddressLine1)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdatedBy).HasMaxLength(250);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<OrderProductType>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductTypeId }).HasName("PK__OrderPro__398349590C305CC1");

            entity.ToTable("OrderProductType", "hand");

            entity.Property(e => e.OrderId)
                .HasMaxLength(250)
                .HasColumnName("OrderID");
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy).HasMaxLength(250);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderProductTypes)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderProd__Order__47A6A41B");

            entity.HasOne(d => d.ProductType).WithMany(p => p.OrderProductTypes)
                .HasForeignKey(d => d.ProductTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderProd__Produ__489AC854");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.StatusId }).HasName("PK__OrderSta__EF1EB9ABA9D17463");

            entity.ToTable("OrderStatus", "hand");

            entity.Property(e => e.OrderId)
                .HasMaxLength(250)
                .HasColumnName("OrderID");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderStatuses)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderStatus_Order");

            entity.HasOne(d => d.Status).WithMany(p => p.OrderStatuses)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderStatus_Status");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.HasKey(e => e.ProductTypeId).HasName("PK__OrderTyp__23AC266CFE6FD8C6");

            entity.ToTable("ProductType", "hand");

            entity.Property(e => e.Code)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy).HasMaxLength(250);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__OrderSta__BC674F41260BEAA2");

            entity.ToTable("Status", "hand");

            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<UserAddress>(entity =>
        {
            entity.HasKey(e => e.UserAddressId).HasName("PK__UserAddr__5961BB971CACB9AE");

            entity.ToTable("UserAddress", "hand");

            entity.Property(e => e.UserAddressId).HasColumnName("UserAddressID");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Postcode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy).HasMaxLength(250);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("UserID");
        });

        modelBuilder.Entity<UserVehicleType>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.VehicleTypeId }).HasName("PK__UserVehi__3E7C85CE26A20837");

            entity.ToTable("UserVehicleType", "hand");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.VehicleTypeId).HasColumnName("VehicleTypeID");
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy).HasMaxLength(250);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VehicleType>(entity =>
        {
            entity.HasKey(e => e.VehicleTypeId).HasName("PK__VehicleT__9F449623358BA952");

            entity.ToTable("VehicleType", "hand");

            entity.Property(e => e.VehicleTypeId)
                .ValueGeneratedNever()
                .HasColumnName("VehicleTypeID");
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.TypeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy).HasMaxLength(250);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
