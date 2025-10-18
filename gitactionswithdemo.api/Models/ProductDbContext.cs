using Microsoft.EntityFrameworkCore;

public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
{
    public DbSet<Product> Product { get; set; } = default!;

}
