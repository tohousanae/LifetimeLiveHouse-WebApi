
using Microsoft.EntityFrameworkCore;

<<<<<<<< HEAD:HatsuneMikuShopWebAPI/Models/MikuMusicShopContext.cs
namespace HatsuneMikuShopWebAPI.Models
========
namespace HatsuneMikuShopAPI.Models
>>>>>>>> abfe7156f0424137ca89cb0a14e9dcaa344ae50c:HatsuneMikuShopAPI/HatsuneMikuShopAPI/Models/MikuMusicShopContext.cs
{
    public class MikuMusicShopContext(DbContextOptions<MikuMusicShopContext> options) : DbContext(options)
    {
        public DbSet<Coupon> Coupon {  get; set; }
        public DbSet<DeliveryBoy> DeliveryBoy { get; set; }
        public DbSet<OrderForm> OrderForm { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Store> Store { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<HeadPicture> HeadPicture { get; set; }   
    }
}
