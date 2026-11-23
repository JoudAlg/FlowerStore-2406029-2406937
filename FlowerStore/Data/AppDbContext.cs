using FlowerStore.Models;
using Microsoft.EntityFrameworkCore;

namespace FlowerStore.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Student 1: products table
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


        // Student 2: Users table
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Starting products, so the store is not empty the first time it runs
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Red Rose Bouquet", Description = "12 Fresh Red Roses with Custom Arrangement", Price = 120, ImageUrl = "/images/flower.jpg" },
                new Product { Id = 2, Name = "Roses with Chocolate", Description = "Luxury Flower Mix with Premium Chocolate Box", Price = 200, ImageUrl = "/images/flower2.png" },
                new Product { Id = 3, Name = "White Lily Bouquet", Description = "Elegant White Lilies Wrapped in Premium Paper", Price = 150 },
                new Product { Id = 4, Name = "Tulip Gift Box", Description = "Colorful Tulips in a Luxury Gift Box", Price = 180 }
            );
        }
    }
}
