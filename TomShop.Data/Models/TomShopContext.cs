using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace TomShop.Data.Models
{
    public partial class TomShopContext : DbContext
    {
        public TomShopContext(DbContextOptions<TomShopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TCart> TCart { get; set; }
        public virtual DbSet<TCartDetail> TCartDetail { get; set; }
        public virtual DbSet<TCategory> TCategory { get; set; }
        public virtual DbSet<TProduct> TProduct { get; set; }
        public virtual DbSet<TTransaction> TTransaction { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TCart>(entity =>
            {
                entity.Property(e => e.CreatedTime).HasPrecision(2);
            });

            modelBuilder.Entity<TCartDetail>(entity =>
            {
                entity.Property(e => e.CartCreatedTime).HasPrecision(2);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<TCategory>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.ImagePath).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TProduct>(entity =>
            {
                entity.Property(e => e.CreatedTime).HasPrecision(2);

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.ImagePath).HasMaxLength(100);

                entity.Property(e => e.Location).HasMaxLength(50);

                entity.Property(e => e.NameFull)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasComment("SQL_Latin1_General_CP1_CI_AI");

                entity.Property(e => e.Note).HasMaxLength(500);

                entity.Property(e => e.VendorProductCode).HasMaxLength(50);

                entity.Property(e => e.VendorProductName).HasMaxLength(500);
            });

            modelBuilder.Entity<TTransaction>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedTime).HasPrecision(2);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
