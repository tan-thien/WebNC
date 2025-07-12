using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.Entities;

namespace WebShopSolution.Data.EF
{
    public class WebShopDbContext : DbContext
    {
        public WebShopDbContext(DbContextOptions<WebShopDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductVariantAttribute> ProductVariantAttributes { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<CustomerVoucher> CustomerVouchers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình quan hệ 1:1 giữa Account và Customer
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Customer)
                .WithOne(c => c.Account)
                .HasForeignKey<Customer>(c => c.IdAcc);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.Parent)
                .WithMany(c => c.Children)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict); // Tránh xoá cascade gây vòng lặp

            //cấu hình quan hệ giữa các voucher
            // ▸ Voucher.Code UNIQUE
            modelBuilder.Entity<Voucher>()
                .HasIndex(v => v.Code)
                .IsUnique();

            // ▸ CustomerVoucher:
            modelBuilder.Entity<CustomerVoucher>()
                .HasOne(cv => cv.Customer)
                .WithMany(c => c.CustomerVouchers)
                .HasForeignKey(cv => cv.IdCus)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CustomerVoucher>()
                .HasOne(cv => cv.Voucher)
                .WithMany(v => v.CustomerVouchers)
                .HasForeignKey(cv => cv.VoucherId)
                .OnDelete(DeleteBehavior.Cascade);

            // (tuỳ chọn) cặp CustomerId + VoucherId UNIQUE -> ngăn trùng
            modelBuilder.Entity<CustomerVoucher>()
                .HasIndex(cv => new { cv.IdCus, cv.VoucherId })
                .IsUnique();
            // Nếu có Fluent API khác thì đặt tại đây
        }
    }

}
