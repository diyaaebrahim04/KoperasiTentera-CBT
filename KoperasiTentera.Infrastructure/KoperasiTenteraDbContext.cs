using System;
using KoperasiTentera.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KoperasiTentera.Infrastructure;
public class KoperasiTenteraDbContext : DbContext
{
    public KoperasiTenteraDbContext(DbContextOptions<KoperasiTenteraDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>()
            .HasIndex(u => u.EmailAddress)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.MobileNumber)
            .IsUnique();

        // Apply default values
        modelBuilder.Entity<User>()
            .Property(u => u.IsMobileVerified)
            .HasDefaultValue(false);

        modelBuilder.Entity<User>()
            .Property(u => u.IsEmailVerified)
            .HasDefaultValue(false);

        modelBuilder.Entity<User>()
            .Property(u => u.IsFingerprintEnabled)
            .HasDefaultValue(false);

        modelBuilder.Entity<User>()
            .Property(u => u.IsFaceIdEnabled)
            .HasDefaultValue(false);

        modelBuilder.Entity<User>()
            .Property(u => u.HashedPin)
            .IsRequired(false);
    }
}