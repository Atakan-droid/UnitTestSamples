﻿using Microsoft.EntityFrameworkCore;

namespace UnitTestMVC.Models
{
    public partial class DbForUntTestContext : DbContext
    {
        public DbForUntTestContext()
        {
        }

        public DbForUntTestContext(DbContextOptions<DbForUntTestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Color).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            });

            //modelBuilder.Entity<Category>().HasData(new Category { Id = 1, Name = "Kalemler" }, new Category { Id = 2, Name = "Defterler" });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
