using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class DataSeeder : IDataSeeder
{
    private readonly IServiceProvider _services;

    public DataSeeder(IServiceProvider services)
    {
        _services = services;
    }

    public void Seed()
    {
        using var scope = _services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

        try
        {
            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: Database migration failed: {ex.Message}");
        }

        if (context.Product.Any())
        {
            return;
        }

        context.Product.AddRange(
            new Product
            {
                Name = "Contoso T-Shirt",
                Price = 19.99m,
                Description = "A breathable, premium cotton T-Shirt crafted for everyday comfort. The fabric is pre-washed to reduce shrinkage and features reinforced seams for long-lasting wear. Ideal for layering or wearing on its own, this tee balances softness with durability.",
                ImageUrl = "/images/products/p-1001.svg"
            },
            new Product
            {
                Name = "Contoso Hoodie",
                Price = 49.99m,
                Description = "This Contoso Hoodie combines a soft fleece lining with a sturdy outer knit to keep you warm without feeling bulky. It includes a roomy kangaroo pocket, adjustable drawstring hood, and ribbed cuffs for a snug fit. Perfect for cool mornings and relaxed weekends.",
                ImageUrl = "/images/products/p-1002.svg"
            },
            new Product
            {
                Name = "Contoso Cap",
                Price = 14.99m,
                Description = "An adjustable baseball cap designed for all-day comfort. The breathable cotton twill keeps you cool, while the adjustable strap ensures a secure fit. Subtle branding makes it a versatile accessory for both casual and outdoor activities.",
                ImageUrl = "/images/products/p-1003.svg"
            },
            new Product
            {
                Name = "Contoso Socks",
                Price = 7.99m,
                Description = "A pack of three lightweight, cushioned socks engineered for comfort and durability. Reinforced heel and toe areas reduce wear, while moisture-wicking fibers help keep feet dry during daily activities. Designed to fit snugly and stay in place.",
                ImageUrl = "/images/products/p-1004.svg"
            },
            new Product
            {
                Name = "Contoso Mug",
                Price = 12.99m,
                Description = "A classic ceramic mug with a comfortable handle and a durable glaze that resists chips. It holds a generous serving of your favorite hot beverage and is microwave and dishwasher safe for convenient everyday use. The crisp design looks great on any kitchen shelf.",
                ImageUrl = "/images/products/p-1005.svg"
            },
            new Product
            {
                Name = "Contoso Backpack",
                Price = 69.99m,
                Description = "A rugged, multipurpose backpack built for urban commutes and weekend adventures. It features multiple compartments for organization, a padded laptop sleeve, and ergonomic straps for comfortable carrying. Water-resistant fabric protects your gear in light rain.",
                ImageUrl = "/images/products/p-1006.svg"
            }
        );

        context.SaveChanges();
    }
}
