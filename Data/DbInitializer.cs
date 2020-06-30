using ProductsRESTApi.Models;
using System.Linq;

namespace ProductsRESTApi.Data
{
    public class DbInitializer
    {
        public static void Initialize(ProductsContext context)
        {
            context.Database.EnsureCreated();

            // Look for any products.
            if (context.Products.Any())
            {
                return;   // DB has been seeded
            }

            var products = new Product[]
            {
                new Product{Name="iPhone 11", Description="A very cool phone", Price=1277.00m, DeliveryPrice=9.00m},
                new Product{Name="iPhone 7", Description="Reliable", Price=489.00m, DeliveryPrice=9.00m},
                new Product{Name="Samsung Galaxy S20+", Description="Truly Cosmic", Price=1444.00m, DeliveryPrice=9.00m},
                new Product{Name="Samsung Galaxy S20 Ultra", Description="Ultra Cosmic", Price=1997.00m, DeliveryPrice=9.00m},
                new Product{Name="Google Pixel 4 XL", Description="Awesome phone", Price=1229.00m, DeliveryPrice=9.00m},
            };

            context.Products.AddRange(products);
            context.SaveChanges();

            var productOptions = new ProductOption[]
            {
                new ProductOption{ProductId=context.Products.Where(p => p.Name == "iPhone 11").Select(p => p.Id ).FirstOrDefault(), Name="Colour", Description="Red"},
                new ProductOption{ProductId=context.Products.Where(p => p.Name == "iPhone 11").Select(p => p.Id ).FirstOrDefault(), Name="Capacity", Description="128Gb"},
                new ProductOption{ProductId=context.Products.Where(p => p.Name == "iPhone 7").Select(p => p.Id ).FirstOrDefault(), Name="Colour", Description="Black"},
                new ProductOption{ProductId=context.Products.Where(p => p.Name == "iPhone 7").Select(p => p.Id ).FirstOrDefault(), Name="Capacity", Description="32Gb"},
                new ProductOption{ProductId=context.Products.Where(p => p.Name == "Samsung Galaxy S20+").Select(p => p.Id ).FirstOrDefault(), Name="Colour", Description="Cloud Blue"},
                new ProductOption{ProductId=context.Products.Where(p => p.Name == "Samsung Galaxy S20+").Select(p => p.Id ).FirstOrDefault(), Name="Capacity", Description="128Gb"},
                new ProductOption{ProductId=context.Products.Where(p => p.Name == "Samsung Galaxy S20 Ultra").Select(p => p.Id ).FirstOrDefault(), Name="Colour", Description="Cosmic Black"},
                new ProductOption{ProductId=context.Products.Where(p => p.Name == "Samsung Galaxy S20 Ultra").Select(p => p.Id ).FirstOrDefault(), Name="Capacity", Description="128Gb"},
                new ProductOption{ProductId=context.Products.Where(p => p.Name == "Google Pixel 4 XL").Select(p => p.Id ).FirstOrDefault(), Name="Colour", Description="Clearly White"},
                new ProductOption{ProductId=context.Products.Where(p => p.Name == "Google Pixel 4 XL").Select(p => p.Id ).FirstOrDefault(), Name="Capacity", Description="128Gb"},
            };

            context.ProductOptions.AddRange(productOptions);
            context.SaveChanges();
        }
    }
}