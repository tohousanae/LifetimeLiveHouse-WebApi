using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace am3burger.Models;

public partial class Am3burgerContext : DbContext
{
    public Am3burgerContext()
    {
    }

    public Am3burgerContext(DbContextOptions<Am3burgerContext> options)
        : base(options)
    {
    }

    // code first資料庫DbSet
    public virtual DbSet<User> User { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<Store> Store { get; set; }
    public DbSet<DeliveryBoy> DeliveryBoy { get; set; }
    public DbSet<Coupon> Coupon { get; set; }
    public DbSet<OrderForm> OrderForm { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot Configuration =
                new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(
                Configuration.GetConnectionString("am3burger"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.Sex).HasMaxLength(10);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
