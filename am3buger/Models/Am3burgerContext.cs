using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;

namespace am3burger.Models
{
    public class Am3burgerContext : DbContext
    {
        // 資料庫索引教學：https://database.klab.tw/lesson-6-2/#:~:text=%E8%B3%87%E6%96%99%E8%A1%A8%E6%9C%AC%E8%BA%AB%E5%8D%B3%E6%98%AF%E4%BE%9D%E6%AD%A4%E7%B4%A2%E5%BC%95%E7%9A%84%E9%A0%86%E5%BA%8F%E5%AD%98%E6%94%BE%EF%BC%8C%E7%84%A1%E9%A0%88%E5%8F%A6%E5%A4%96%E5%BB%BA%E7%B4%A2%E5%BC%95%E8%A1%A8%EF%BC%8C%E5%9B%A0%E6%AD%A4%E6%AF%8F%E5%80%8B%E8%A1%A8%E5%8F%AA%E8%83%BD%E6%9C%89%E4%B8%80%E5%80%8B%E5%8F%A2%E9%9B%86%E7%B4%A2%E5%BC%95%EF%BC%8C%E9%80%9A%E5%B8%B8%E6%98%AF%E4%B8%BB%E9%8D%B5%20%28Primary%20Key%29%E3%80%82%20%E7%89%B9%E6%80%A7%E6%98%AF%E6%9F%A5%E8%A9%A2%E6%9C%80%E5%BF%AB%E3%80%81%E6%9C%80%E7%9B%B4%E6%8E%A5%EF%BC%8C%E7%95%B0%E5%8B%95%E5%8D%BB%E6%9C%80%E6%85%A2%EF%BC%8C%E5%9B%A0%E7%82%BA%E6%AF%8F%E5%80%8B%E6%AC%84%E4%BD%8D%E9%83%BD%E8%A6%81%E6%90%AC%E5%8B%95%E3%80%82,%E5%BB%BA%E7%AB%8B%E5%8F%A2%E9%9B%86%20%28Cluster%29%E7%B4%A2%E5%BC%95%E7%AF%84%E4%BE%8B%E5%A6%82%E4%B8%8B%EF%BC%9A%20on%20product%28pid%29
        public Am3burgerContext(DbContextOptions<Am3burgerContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User資料表的Identity欄位預設值為顧客
            modelBuilder.Entity<User>()
                .Property(b => b.Identity)
                .HasDefaultValue("顧客");

            modelBuilder.Entity<User>()
                .Property(b => b.PhoneValidation)
                .HasDefaultValue(false);

            modelBuilder.Entity<User>()
                .Property(b => b.EmailValidation)
                .HasDefaultValue(false);
        }

        public DbSet<Coupon> Coupon {  get; set; }
        public DbSet<DeliveryBoy> DeliveryBoy { get; set; }
        public DbSet<OrderForm> OrderForm { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Store> Store { get; set; }
        public DbSet<User> User { get; set; }
    }
}
