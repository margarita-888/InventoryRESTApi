using Microsoft.EntityFrameworkCore;
using InventoryRESTApi.Models;
using System;

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
        modelBuilder.Entity<InventoryItem>()
            .HasKey(i => i.Id);

        modelBuilder.Entity<InventoryItemOption>()
            .ToTable("InventoryItemOption")
            .HasOne(o => o.InventoryItem)
            .WithMany(i => i.InventoryItemOptions)
            .OnDelete(DeleteBehavior.Cascade);
    }
}