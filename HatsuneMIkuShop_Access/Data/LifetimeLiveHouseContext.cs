using LifetimeLiveHouse.Models;
using Microsoft.EntityFrameworkCore;

namespace LifetimeLiveHouse.Access.Data
{
    public class LifetimeLiveHouseContext(DbContextOptions<LifetimeLiveHouseContext> options) : DbContext(options)
    {
        public virtual DbSet<Coupon> Coupon {  get; set; }
        public virtual DbSet<DeliveryBoy> DeliveryBoy { get; set; }
        public virtual DbSet<OrderForm> OrderForm { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Store> Store { get; set; }
        public virtual DbSet<Member> User { get; set; }
    }
}
