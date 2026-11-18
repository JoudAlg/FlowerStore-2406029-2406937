using Microsoft.EntityFrameworkCore;

namespace FlowerStore.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Student 1 adds:  public DbSet<Product> Products { get; set; }

        // Student 2 adds:  public DbSet<User> Users { get; set; }
    }
}
