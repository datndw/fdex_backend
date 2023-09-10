﻿using System;
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

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(u => u.Id);
            });
        }

        public DbSet<Transaction> Transactions { get; set; }
    }
}
