using Microsoft.EntityFrameworkCore;

namespace Product.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Models.Product> Products { get; set; }
    }
}
