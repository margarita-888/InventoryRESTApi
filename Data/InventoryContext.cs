using Microsoft.EntityFrameworkCore;
using InventoryRESTApi.Models;

public class InventoryContext : DbContext
{
    public InventoryContext(DbContextOptions<InventoryContext> options)
        : base(options)
    {
    }

    public DbSet<InventoryItem> InventoryItem { get; set; }
    public DbSet<InventoryItemOption> InventoryItemOptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InventoryItemOption>()
           .ToTable("InventoryItemOption")
       .HasOne(po => po.InventoryItem)
       .WithMany(p => p.InventoryItemOptions)
       .OnDelete(DeleteBehavior.Cascade);
    }
}