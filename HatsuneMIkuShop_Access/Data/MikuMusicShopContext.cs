using HatsuneMIkuShop.Models;
using Microsoft.EntityFrameworkCore;

namespace HatsuneMIkuShop.Access.Data
{
    public class MikuMusicShopContext(DbContextOptions<MikuMusicShopContext> options) : DbContext(options)
    {
        public virtual DbSet<Coupon> Coupon {  get; set; }
        public virtual DbSet<DeliveryBoy> DeliveryBoy { get; set; }
        public virtual DbSet<OrderForm> OrderForm { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Store> Store { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<HeadPicture> HeadPicture { get; set; }   
    }
}
