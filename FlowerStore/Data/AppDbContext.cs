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

            // Products added before categories existed get "Bouquets"
            modelBuilder.Entity<Product>()
                .Property(p => p.Category)
                .HasDefaultValue("Bouquets");

            // Starting products, so the store is not empty the first time it runs
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Red Rose Bouquet", Description = "12 Fresh Red Roses with Custom Arrangement", Price = 120, ImageUrl = "/images/red-roses.jpg", Category = "Roses" },
                new Product { Id = 2, Name = "Roses with Chocolate", Description = "Luxury Flower Mix with Premium Chocolate Box", Price = 200, ImageUrl = "/images/roses-chocolate.jpg", Category = "Gift Boxes" },
                new Product { Id = 3, Name = "White Lily Bouquet", Description = "Elegant White Lilies Wrapped in Premium Paper", Price = 150, ImageUrl = "/images/white-lily.jpg", Category = "Bouquets" },
                new Product { Id = 4, Name = "Tulip Gift Box", Description = "Colorful Tulips in a Luxury Gift Box", Price = 180, ImageUrl = "/images/tulip-box.jpg", Category = "Gift Boxes" }
            );
        }
    }
}
