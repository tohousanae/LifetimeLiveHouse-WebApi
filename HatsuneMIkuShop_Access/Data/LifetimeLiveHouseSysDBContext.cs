using LifetimeLiveHouse.Models;
using Microsoft.EntityFrameworkCore;

namespace LifetimeLiveHouse.Access.Data
{
    public class LifetimeLiveHouseSysDBContext : DbContext
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

        public virtual DbSet<MemberPicture> MemberPicture { get; set; }

        public virtual DbSet<MemberStatus> MemberStatus { get; set; }

        public virtual DbSet<MemberVerificationStatus> MemberVerificationStatus { get; set; }

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
            base.OnModelCreating(modelBuilder);

            // Member → Event
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Member)
                .WithMany(m => m.Events)
                .HasForeignKey(e => e.MemberID)
                .OnDelete(DeleteBehavior.Restrict);

            // Event → RegisteredEvent
            modelBuilder.Entity<RegisteredEvent>()
                .HasOne(re => re.Event)
                .WithMany(e => e.RegisteredEvents)
                .HasForeignKey(re => re.EventID)
                .OnDelete(DeleteBehavior.Cascade);

            // Member → RegisteredEvent
            modelBuilder.Entity<RegisteredEvent>()
                .HasOne(re => re.Member)
                .WithMany(m => m.RegisteredEvents)
                .HasForeignKey(re => re.MemberID)
                .OnDelete(DeleteBehavior.NoAction);
        }

    }

}