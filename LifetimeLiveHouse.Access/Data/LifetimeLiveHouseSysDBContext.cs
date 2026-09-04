using System;
using System.Collections.Generic;
using LifetimeLiveHouse.Models;
using Microsoft.EntityFrameworkCore;

namespace LifetimeLiveHouse.Access.Data;

public partial class LifetimeLiveHouseSysDBContext : DbContext
{
    public LifetimeLiveHouseSysDBContext(DbContextOptions<LifetimeLiveHouseSysDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AttendanceRecord> AttendanceRecord { get; set; }

    public virtual DbSet<BandRole> BandRole { get; set; }

    public virtual DbSet<Cart> Cart { get; set; }

    public virtual DbSet<Category> Category { get; set; }

    public virtual DbSet<Coupon> Coupon { get; set; }

    public virtual DbSet<Employee> Employee { get; set; }

    public virtual DbSet<EmployeeAccount> EmployeeAccount { get; set; }

    public virtual DbSet<EmployeeRole> EmployeeRole { get; set; }

    public virtual DbSet<Event> Event { get; set; }

    public virtual DbSet<EventStatus> EventStatus { get; set; }

    public virtual DbSet<Instrument> Instrument { get; set; }

    public virtual DbSet<Live> Live { get; set; }

    public virtual DbSet<LoginRecord> LoginRecord { get; set; }

    public virtual DbSet<Member> Member { get; set; }

    public virtual DbSet<MemberAccount> MemberAccount { get; set; }

    public virtual DbSet<MemberEmailVerificationStatus> MemberEmailVerificationStatus { get; set; }

    public virtual DbSet<MemberHeadPicture> MemberHeadPicture { get; set; }

    public virtual DbSet<MemberPhoneVerificationStatus> MemberPhoneVerificationStatus { get; set; }

    public virtual DbSet<MemberPicture> MemberPicture { get; set; }

    public virtual DbSet<MemberStatus> MemberStatus { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Notification> Notification { get; set; }

    public virtual DbSet<Order> Order { get; set; }

    public virtual DbSet<OrderDetail> OrderDetail { get; set; }

    public virtual DbSet<OrderStatus> OrderStatus { get; set; }

    public virtual DbSet<PasswordResetToken> PasswordResetToken { get; set; }

    public virtual DbSet<PayType> PayType { get; set; }

    public virtual DbSet<Product> Product { get; set; }

    public virtual DbSet<ProductStatus> ProductStatus { get; set; }

    public virtual DbSet<RegisteredEvent> RegisteredEvent { get; set; }

    public virtual DbSet<RehearsalStudio> RehearsalStudio { get; set; }

    public virtual DbSet<Seat> Seat { get; set; }

    public virtual DbSet<ShippingMethod> ShippingMethod { get; set; }

    public virtual DbSet<Store> Store { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AttendanceRecord>(entity =>
        {
            entity.HasKey(e => e.AttendanceID);

            entity.HasIndex(e => e.EmployeeID, "IX_AttendanceRecord_EmployeeID");

            entity.HasOne(d => d.Employee).WithMany(p => p.AttendanceRecord).HasForeignKey(d => d.EmployeeID);
        });

        modelBuilder.Entity<BandRole>(entity =>
        {
            entity.Property(e => e.BandRoleID)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.Role).HasMaxLength(20);
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasIndex(e => e.MemberID, "IX_Cart_MemberID");

            entity.HasIndex(e => e.ProductID, "IX_Cart_ProductID");

            entity.HasOne(d => d.Member).WithMany(p => p.Cart).HasForeignKey(d => d.MemberID);

            entity.HasOne(d => d.Product).WithMany(p => p.Cart).HasForeignKey(d => d.ProductID);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CateID);

            entity.Property(e => e.CateID)
                .HasMaxLength(5)
                .IsFixedLength();
            entity.Property(e => e.CateName).HasMaxLength(20);
        });

        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(e => e.cNo);

            entity.HasIndex(e => e.MemberID, "IX_Coupon_MemberID");

            entity.HasIndex(e => e.ProductID, "IX_Coupon_ProductID");

            entity.Property(e => e.Discount).HasColumnType("money");
            entity.Property(e => e.GetCouponDate).HasDefaultValueSql("(getutcdate())", "DF_Coupon_GetDate");
            entity.Property(e => e.cDesc).HasMaxLength(200);

            entity.HasOne(d => d.Member).WithMany(p => p.Coupon).HasForeignKey(d => d.MemberID);

            entity.HasOne(d => d.Product).WithMany(p => p.Coupon).HasForeignKey(d => d.ProductID);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasIndex(e => e.RoleCode, "IX_Employee_RoleCode");

            entity.HasIndex(e => e.StoreID, "IX_Employee_StoreID");

            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(40);
            entity.Property(e => e.RoleCode).HasMaxLength(1);
            entity.Property(e => e.Tel).HasMaxLength(20);

            entity.HasOne(d => d.RoleCodeNavigation).WithMany(p => p.Employee).HasForeignKey(d => d.RoleCode);

            entity.HasOne(d => d.Store).WithMany(p => p.Employee).HasForeignKey(d => d.StoreID);
        });

        modelBuilder.Entity<EmployeeAccount>(entity =>
        {
            entity.HasKey(e => e.Email);

            entity.HasIndex(e => e.EmployeeID, "IX_EmployeeAccount_EmployeeID").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(30);
            entity.Property(e => e.Password).HasMaxLength(200);

            entity.HasOne(d => d.Employee).WithOne(p => p.EmployeeAccount).HasForeignKey<EmployeeAccount>(d => d.EmployeeID);
        });

        modelBuilder.Entity<EmployeeRole>(entity =>
        {
            entity.HasKey(e => e.RoleCode);

            entity.Property(e => e.RoleCode).HasMaxLength(1);
            entity.Property(e => e.RoleName).HasMaxLength(15);
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasIndex(e => e.MemberID, "IX_Event_MemberID");

            entity.HasIndex(e => e.StatusCode, "IX_Event_StatusCode");

            entity.HasIndex(e => e.StoreID, "IX_Event_StoreID");

            entity.Property(e => e.EventName).HasMaxLength(40);
            entity.Property(e => e.EventPicture).HasMaxLength(50);
            entity.Property(e => e.RegistrationFee).HasColumnType("money");
            entity.Property(e => e.StatusCode)
                .HasMaxLength(1)
                .IsFixedLength();

            entity.HasOne(d => d.Member).WithMany(p => p.Event)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.StatusCodeNavigation).WithMany(p => p.Event).HasForeignKey(d => d.StatusCode);

            entity.HasOne(d => d.Store).WithMany(p => p.Event).HasForeignKey(d => d.StoreID);
        });

        modelBuilder.Entity<EventStatus>(entity =>
        {
            entity.HasKey(e => e.StatusCode);

            entity.Property(e => e.StatusCode)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.Status).HasMaxLength(10);
        });

        modelBuilder.Entity<Instrument>(entity =>
        {
            entity.HasIndex(e => e.MemberID, "IX_Instrument_MemberID");

            entity.HasIndex(e => e.StoreID, "IX_Instrument_StoreID");

            entity.Property(e => e.InstrumentName).HasMaxLength(40);
            entity.Property(e => e.InstrumentPhoto).HasMaxLength(50);
            entity.Property(e => e.RentFeePerHour).HasColumnType("money");

            entity.HasOne(d => d.Member).WithMany(p => p.Instrument).HasForeignKey(d => d.MemberID);

            entity.HasOne(d => d.Store).WithMany(p => p.Instrument).HasForeignKey(d => d.StoreID);
        });

        modelBuilder.Entity<Live>(entity =>
        {
            entity.HasIndex(e => e.BandRoleID, "IX_Live_BandRoleID");

            entity.HasIndex(e => e.EventStatusStatusCode, "IX_Live_EventStatusStatusCode");

            entity.HasIndex(e => e.MemberID, "IX_Live_MemberID");

            entity.HasIndex(e => e.StoreID, "IX_Live_StoreID");

            entity.Property(e => e.Admission).HasColumnType("money");
            entity.Property(e => e.BandRoleID)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.EventStatusStatusCode)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.LiveName).HasMaxLength(40);
            entity.Property(e => e.LiveSong).HasMaxLength(40);

            entity.HasOne(d => d.BandRole).WithMany(p => p.Live).HasForeignKey(d => d.BandRoleID);

            entity.HasOne(d => d.EventStatusStatusCodeNavigation).WithMany(p => p.Live).HasForeignKey(d => d.EventStatusStatusCode);

            entity.HasOne(d => d.Member).WithMany(p => p.Live).HasForeignKey(d => d.MemberID);

            entity.HasOne(d => d.Store).WithMany(p => p.Live).HasForeignKey(d => d.StoreID);
        });

        modelBuilder.Entity<LoginRecord>(entity =>
        {
            entity.HasKey(e => e.RecordID);

            entity.HasIndex(e => e.MemberID, "IX_LoginRecord_MemberID");

            entity.Property(e => e.LoginDate).HasDefaultValueSql("(getutcdate())", "DF_LoginRecord_LoginDate");
            entity.Property(e => e.Record).HasMaxLength(200);

            entity.HasOne(d => d.Member).WithMany(p => p.LoginRecord).HasForeignKey(d => d.MemberID);
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasIndex(e => e.StatusCode, "IX_Member_StatusCode");

            entity.Property(e => e.Cash).HasColumnType("money");
            entity.Property(e => e.CellphoneNumber).HasMaxLength(20);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())", "DF_Member_CreatedDate");
            entity.Property(e => e.Name).HasMaxLength(40);
            entity.Property(e => e.StatusCode)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasDefaultValueSql("((0))", "DF_Member_StatusCode");

            entity.HasOne(d => d.StatusCodeNavigation).WithMany(p => p.Member).HasForeignKey(d => d.StatusCode);
        });

        modelBuilder.Entity<MemberAccount>(entity =>
        {
            entity.HasKey(e => e.Email);

            entity.HasIndex(e => e.MemberID, "IX_MemberAccount_MemberID");

            entity.Property(e => e.Email).HasMaxLength(30);
            entity.Property(e => e.Password).HasMaxLength(200);

            entity.HasOne(d => d.Member).WithMany(p => p.MemberAccount).HasForeignKey(d => d.MemberID);
        });

        modelBuilder.Entity<MemberEmailVerificationStatus>(entity =>
        {
            entity.HasKey(e => e.MemberID);

            entity.Property(e => e.MemberID).ValueGeneratedNever();
            entity.Property(e => e.EmailVerificationTokenExpiry).HasDefaultValueSql("(dateadd(hour,(24),getutcdate()))", "DF_MemberEmailVerificationStatus_EmailVerificationTokenExpiry");

            entity.HasOne(d => d.Member).WithOne(p => p.MemberEmailVerificationStatus).HasForeignKey<MemberEmailVerificationStatus>(d => d.MemberID);
        });

        modelBuilder.Entity<MemberHeadPicture>(entity =>
        {
            entity.HasKey(e => e.Picture);

            entity.HasIndex(e => e.MemberID, "IX_MemberHeadPicture_MemberID").IsUnique();

            entity.Property(e => e.Picture).HasMaxLength(50);

            entity.HasOne(d => d.Member).WithOne(p => p.MemberHeadPicture).HasForeignKey<MemberHeadPicture>(d => d.MemberID);
        });

        modelBuilder.Entity<MemberPhoneVerificationStatus>(entity =>
        {
            entity.HasKey(e => e.MemberID);

            entity.Property(e => e.MemberID).ValueGeneratedNever();

            entity.HasOne(d => d.Member).WithOne(p => p.MemberPhoneVerificationStatus).HasForeignKey<MemberPhoneVerificationStatus>(d => d.MemberID);
        });

        modelBuilder.Entity<MemberPicture>(entity =>
        {
            entity.HasKey(e => e.Picture);

            entity.HasIndex(e => e.MemberID, "IX_MemberPicture_MemberID");

            entity.Property(e => e.Picture).HasMaxLength(50);

            entity.HasOne(d => d.Member).WithMany(p => p.MemberPicture).HasForeignKey(d => d.MemberID);
        });

        modelBuilder.Entity<MemberStatus>(entity =>
        {
            entity.HasKey(e => e.StatusCode);

            entity.Property(e => e.StatusCode)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.Status).HasMaxLength(10);
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.Property(e => e.Author).HasMaxLength(10);
            entity.Property(e => e.NewsTitle).HasMaxLength(40);
            entity.Property(e => e.PostDate).HasDefaultValueSql("(getutcdate())", "DF_News_PostDate");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasIndex(e => e.MemberID, "IX_Notification_MemberID");

            entity.Property(e => e.Description).HasMaxLength(200);

            entity.HasOne(d => d.Member).WithMany(p => p.Notification).HasForeignKey(d => d.MemberID);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasIndex(e => e.EmployeeID, "IX_Order_EmployeeID");

            entity.HasIndex(e => e.MemberID, "IX_Order_MemberID");

            entity.HasIndex(e => e.PayCode, "IX_Order_PayCode");

            entity.HasIndex(e => e.StatusCode, "IX_Order_StatusCode");

            entity.Property(e => e.Note).HasMaxLength(200);
            entity.Property(e => e.OrderDate).HasDefaultValueSql("(getutcdate())", "DF_Order_OrderDate");
            entity.Property(e => e.PayCode)
                .HasMaxLength(2)
                .IsFixedLength();
            entity.Property(e => e.StatusCode).HasMaxLength(1);
            entity.Property(e => e.oTel).HasMaxLength(20);

            entity.HasOne(d => d.Employee).WithMany(p => p.Order).HasForeignKey(d => d.EmployeeID);

            entity.HasOne(d => d.Member).WithMany(p => p.Order).HasForeignKey(d => d.MemberID);

            entity.HasOne(d => d.PayCodeNavigation).WithMany(p => p.Order).HasForeignKey(d => d.PayCode);

            entity.HasOne(d => d.StatusCodeNavigation).WithMany(p => p.Order).HasForeignKey(d => d.StatusCode);

            entity.HasMany(d => d.OrderDetail).WithMany(p => p.OrdersOrder)
                .UsingEntity<Dictionary<string, object>>(
                    "OrderOrderDetail",
                    r => r.HasOne<OrderDetail>().WithMany().HasForeignKey("OrderDetailOrderID", "OrderDetailProductID"),
                    l => l.HasOne<Order>().WithMany().HasForeignKey("OrdersOrderID"),
                    j =>
                    {
                        j.HasKey("OrdersOrderID", "OrderDetailOrderID", "OrderDetailProductID");
                        j.HasIndex(new[] { "OrderDetailOrderID", "OrderDetailProductID" }, "IX_OrderOrderDetail_OrderDetailOrderID_OrderDetailProductID");
                    });
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderID, e.ProductID });

            entity.HasIndex(e => e.ShippingMethodCode, "IX_OrderDetail_ShippingMethodCode");

            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.ShippingAddress).HasMaxLength(50);
            entity.Property(e => e.ShippingMethodCode)
                .HasMaxLength(1)
                .IsFixedLength();

            entity.HasOne(d => d.ShippingMethodCodeNavigation).WithMany(p => p.OrderDetail).HasForeignKey(d => d.ShippingMethodCode);
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.StatusCode);

            entity.Property(e => e.StatusCode).HasMaxLength(1);
            entity.Property(e => e.Status).HasMaxLength(10);
        });

        modelBuilder.Entity<PasswordResetToken>(entity =>
        {
            entity.HasIndex(e => e.MemberID, "IX_PasswordResetToken_MemberID");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())", "DF_PasswordResetToken_CreatedAt");
            entity.Property(e => e.ExpiresAt).HasDefaultValueSql("(dateadd(hour,(1),getutcdate()))", "DF_PasswordResetToken_ExpiresAt");

            entity.HasOne(d => d.Member).WithMany(p => p.PasswordResetToken).HasForeignKey(d => d.MemberID);
        });

        modelBuilder.Entity<PayType>(entity =>
        {
            entity.HasKey(e => e.PayCode);

            entity.Property(e => e.PayCode)
                .HasMaxLength(2)
                .IsFixedLength();
            entity.Property(e => e.ShippingFee).HasColumnType("money");
            entity.Property(e => e.Type).HasMaxLength(10);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasIndex(e => e.CateID, "IX_Product_CateID");

            entity.HasIndex(e => e.StatusCode, "IX_Product_StatusCode");

            entity.Property(e => e.CateID)
                .HasMaxLength(5)
                .IsFixedLength();
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())", "DF_Product_CreatedDate");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Photo).HasMaxLength(50);
            entity.Property(e => e.Pricing).HasColumnType("money");
            entity.Property(e => e.ProductName).HasMaxLength(40);
            entity.Property(e => e.RetailPrice).HasColumnType("money");
            entity.Property(e => e.StatusCode)
                .HasMaxLength(1)
                .IsFixedLength();

            entity.HasOne(d => d.Cate).WithMany(p => p.Product).HasForeignKey(d => d.CateID);

            entity.HasOne(d => d.StatusCodeNavigation).WithMany(p => p.Product).HasForeignKey(d => d.StatusCode);

            entity.HasMany(d => d.OrderDetail).WithMany(p => p.ProductsProduct)
                .UsingEntity<Dictionary<string, object>>(
                    "OrderDetailProduct",
                    r => r.HasOne<OrderDetail>().WithMany().HasForeignKey("OrderDetailOrderID", "OrderDetailProductID"),
                    l => l.HasOne<Product>().WithMany().HasForeignKey("ProductsProductID"),
                    j =>
                    {
                        j.HasKey("ProductsProductID", "OrderDetailOrderID", "OrderDetailProductID");
                        j.HasIndex(new[] { "OrderDetailOrderID", "OrderDetailProductID" }, "IX_OrderDetailProduct_OrderDetailOrderID_OrderDetailProductID");
                    });
        });

        modelBuilder.Entity<ProductStatus>(entity =>
        {
            entity.HasKey(e => e.StatusCode);

            entity.Property(e => e.StatusCode)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.Status).HasMaxLength(10);
        });

        modelBuilder.Entity<RegisteredEvent>(entity =>
        {
            entity.HasKey(e => e.RecordID);

            entity.HasIndex(e => e.EventID, "IX_RegisteredEvent_EventID");

            entity.HasIndex(e => e.MemberID, "IX_RegisteredEvent_MemberID");

            entity.HasOne(d => d.Event).WithMany(p => p.RegisteredEvent).HasForeignKey(d => d.EventID);

            entity.HasOne(d => d.Member).WithMany(p => p.RegisteredEvent)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<RehearsalStudio>(entity =>
        {
            entity.HasIndex(e => e.MemberID, "IX_RehearsalStudio_MemberID");

            entity.HasIndex(e => e.StoreID, "IX_RehearsalStudio_StoreID");

            entity.Property(e => e.RehearsalStudioName).HasMaxLength(40);
            entity.Property(e => e.RehearsalStudioPhoto).HasMaxLength(50);
            entity.Property(e => e.RentFeePerHour).HasColumnType("money");

            entity.HasOne(d => d.Member).WithMany(p => p.RehearsalStudio).HasForeignKey(d => d.MemberID);

            entity.HasOne(d => d.Store).WithMany(p => p.RehearsalStudio).HasForeignKey(d => d.StoreID);
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasIndex(e => e.MemberID, "IX_Seat_MemberID").IsUnique();

            entity.HasIndex(e => e.StoreID, "IX_Seat_StoreID");

            entity.HasOne(d => d.Member).WithOne(p => p.Seat).HasForeignKey<Seat>(d => d.MemberID);

            entity.HasOne(d => d.Store).WithMany(p => p.Seat).HasForeignKey(d => d.StoreID);
        });

        modelBuilder.Entity<ShippingMethod>(entity =>
        {
            entity.HasKey(e => e.ShippingMethodCode);

            entity.Property(e => e.ShippingMethodCode)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.Method).HasMaxLength(10);
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())", "DF_Store_CreatedDate");
            entity.Property(e => e.RentFeePerHour).HasColumnType("money");
            entity.Property(e => e.StoreName).HasMaxLength(40);
            entity.Property(e => e.sTel).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
