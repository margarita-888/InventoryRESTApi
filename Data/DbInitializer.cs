using InventoryRESTApi.Models;
using System.Linq;

namespace InventoryRESTApi.Data
{
    public class DbInitializer
    {
        public static void Initialize(InventoryContext context)
        {
            context.Database.EnsureCreated();

            // Look for any Inventory.
            if (context.InventoryItem.Any())
            {
                return;   // DB has been seeded
            }

            var inventoryItems = new InventoryItem[]
            {
                new InventoryItem{Name="iPhone 11", Description="A very cool phone", Price=1277.00, DeliveryPrice=9.00},
                new InventoryItem{Name="iPhone 7", Description="Reliable", Price=489.00, DeliveryPrice=9.00},
                new InventoryItem{Name="Samsung Galaxy S20+", Description="Truly Cosmic", Price=1444.00, DeliveryPrice=9.00},
                new InventoryItem{Name="Samsung Galaxy S20 Ultra", Description="Ultra Cosmic", Price=1997.00, DeliveryPrice=9.00},
                new InventoryItem{Name="Google Pixel 4 XL", Description="Awesome phone", Price=1229.00, DeliveryPrice=9.00},
            };

            context.InventoryItem.AddRange(inventoryItems);
            context.SaveChanges();

            var inventoryItemOptions = new InventoryItemOption[]
            {
                new InventoryItemOption{InventoryItemId=context.InventoryItem.Where(p => p.Name == "iPhone 11").Select(p => p.Id ).FirstOrDefault(), Name="Colour", Description="Red"},
                new InventoryItemOption{InventoryItemId=context.InventoryItem.Where(p => p.Name == "iPhone 11").Select(p => p.Id ).FirstOrDefault(), Name="Capacity", Description="128Gb"},
                new InventoryItemOption{InventoryItemId=context.InventoryItem.Where(p => p.Name == "iPhone 7").Select(p => p.Id ).FirstOrDefault(), Name="Colour", Description="Black"},
                new InventoryItemOption{InventoryItemId=context.InventoryItem.Where(p => p.Name == "iPhone 7").Select(p => p.Id ).FirstOrDefault(), Name="Capacity", Description="32Gb"},
                new InventoryItemOption{InventoryItemId=context.InventoryItem.Where(p => p.Name == "Samsung Galaxy S20+").Select(p => p.Id ).FirstOrDefault(), Name="Colour", Description="Cloud Blue"},
                new InventoryItemOption{InventoryItemId=context.InventoryItem.Where(p => p.Name == "Samsung Galaxy S20+").Select(p => p.Id ).FirstOrDefault(), Name="Capacity", Description="128Gb"},
                new InventoryItemOption{InventoryItemId=context.InventoryItem.Where(p => p.Name == "Samsung Galaxy S20 Ultra").Select(p => p.Id ).FirstOrDefault(), Name="Colour", Description="Cosmic Black"},
                new InventoryItemOption{InventoryItemId=context.InventoryItem.Where(p => p.Name == "Samsung Galaxy S20 Ultra").Select(p => p.Id ).FirstOrDefault(), Name="Capacity", Description="128Gb"},
                new InventoryItemOption{InventoryItemId=context.InventoryItem.Where(p => p.Name == "Google Pixel 4 XL").Select(p => p.Id ).FirstOrDefault(), Name="Colour", Description="Clearly White"},
                new InventoryItemOption{InventoryItemId=context.InventoryItem.Where(p => p.Name == "Google Pixel 4 XL").Select(p => p.Id ).FirstOrDefault(), Name="Capacity", Description="128Gb"},
            };

            context.InventoryItemOptions.AddRange(inventoryItemOptions);
            context.SaveChanges();
        }
    }
}