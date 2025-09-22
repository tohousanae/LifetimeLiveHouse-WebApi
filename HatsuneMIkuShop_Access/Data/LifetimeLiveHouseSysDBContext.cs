using LifetimeLiveHouse.Models;
using Microsoft.EntityFrameworkCore;

namespace LifetimeLiveHouse.Access.Data
{
    public class LifetimeLiveHouseSysDBContext(DbContextOptions<LifetimeLiveHouseSysDBContext> options) : DbContext(options)
    {
        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<MemberAccount> MemberAccount { get; set; }
    }
}
