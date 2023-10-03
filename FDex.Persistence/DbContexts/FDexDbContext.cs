using System;
using FDex.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FDex.Persistence.DbContexts
{
	public class FDexDbContext : DbContext
	{
        public FDexDbContext(DbContextOptions<FDexDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FDexDbContext).Assembly);

            modelBuilder.Entity<Swap>(entity =>
            {
                entity.HasKey(u => u.TxnHash);
                entity.HasOne<User>(s => s.User)
                .WithMany(u => u.Swaps)
                .HasForeignKey(u => u.Wallet);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Wallet);
            });

            modelBuilder.Entity<Reporter>(entity =>
            {
                entity.HasKey(u => u.Wallet);
            });
        }

        public DbSet<Swap> Swaps { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Reporter> Reporters { get; set; }
    }
}

