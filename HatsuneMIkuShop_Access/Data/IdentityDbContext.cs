using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class IdentityDbContext : IdentityDbContext<IdentityUser>
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) :
        base(options)
    { }
}