using LifetimeLiveHouse.Models;
using Microsoft.EntityFrameworkCore;

namespace LifetimeLiveHouse.Access.Data
{
    public class LifetimeLiveHouseSysDBContext(DbContextOptions<LifetimeLiveHouseSysDBContext> options) : DbContext(options)
    {
        public virtual DbSet<Coupon> Coupon {  get; set; }
        public virtual DbSet<OrderForm> OrderForm { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Store> Store { get; set; }
        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<Member> MemberAccount { get; set; }
    }
}
