using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;

namespace am3burger.Models
{
    public class Am3burgerContext : DbContext
    {
        public Am3burgerContext(DbContextOptions<Am3burgerContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User資料表的Identity欄位預設值為顧客
            modelBuilder.Entity<User>()
                .Property(b => b.Identity)
                .HasDefaultValue("顧客");
        }

        public DbSet<Coupon> Coupon {  get; set; }
        public DbSet<DeliveryBoy> DeliveryBoy { get; set; }
        public DbSet<OrderForm> OrderForm { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Store> Store { get; set; }
        public DbSet<User> User { get; set; }
    }
}
