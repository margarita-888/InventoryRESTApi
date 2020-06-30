using Microsoft.EntityFrameworkCore;
using ProductsRESTApi.Models;

public class ProductsContext : DbContext
{
    public ProductsContext(DbContextOptions<ProductsContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<ProductOption> ProductOptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductOption>()
           .ToTable("ProductOption")
       .HasOne(po => po.Product)
       .WithMany(p => p.ProductOptions)
       .OnDelete(DeleteBehavior.Cascade);
    }
}