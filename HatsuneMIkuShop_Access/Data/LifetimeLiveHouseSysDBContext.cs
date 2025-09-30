using LifetimeLiveHouse.Models;
using Microsoft.EntityFrameworkCore;

namespace LifetimeLiveHouse.Access.Data
{
    public class LifetimeLiveHouseSysDBContext(DbContextOptions<LifetimeLiveHouseSysDBContext> options) : DbContext(options)
    {
        public virtual DbSet<AttendanceRecord> AttendanceRecord { get; set; }

        public virtual DbSet<BandRole> BandRole { get; set; }

        public virtual DbSet<Cart> Cart { get; set; }

        public virtual DbSet<Category> Category { get; set; }

        public virtual DbSet<Coupon> Coupon { get; set; }

        public virtual DbSet<Employee> Employee { get; set; }

        public virtual DbSet<EmployeeAccount> EmployeeAccount { get; set; }

        public virtual DbSet<EmployeeRole> EmployeeRole { get; set; }

        public virtual DbSet<Store> Store { get; set; }

        public virtual DbSet<Concert> Concert { get; set; }

        public virtual DbSet<Member> Member { get; set; }
        
        public virtual DbSet<MemberAccount> MemberAccount { get; set; }

        public virtual DbSet<Seat> Seat { get; set; }
        
        public virtual DbSet<Product> Product { get; set; }

    }
}
