using System;
using FDex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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
                entity.HasMany(u => u.ReferredUsers)
                    .WithOne()
                    .HasForeignKey(u => u.ReferredUserOf);
            });

            modelBuilder.Entity<Reporter>(entity =>
            {
                entity.HasKey(u => u.Wallet);
            });

            modelBuilder.Entity<AddLiquidity>(entity =>
            {
                entity.HasKey(u => u.TxnHash);
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasKey(u => u.Key);
                entity.HasOne<User>(s => s.User)
                    .WithMany(u => u.Positions)
                    .HasForeignKey(u => u.Wallet);
            });
        }

        public DbSet<Swap> Swaps { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Reporter> Reporters { get; set; }
        public DbSet<AddLiquidity> AddLiquidities { get; set; }
        public DbSet<Position> Positions { get; set; }
    }
}

