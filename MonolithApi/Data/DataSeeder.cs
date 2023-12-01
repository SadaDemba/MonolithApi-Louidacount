using AutoBogus;
using Bogus;
using MonolithApi.Context;
using MonolithApi.Models;

namespace MonolithApi.Data
{
    public class DataSeeder
    {

        public static void Initialize(AppDatabaseContext context)
        {
            if (!context.Addresses.Any())
            {
                var addressFaker = new Faker<Address>("fr")
                   .RuleFor(a => a.StreetNumber, f => int.Parse(f.Address.BuildingNumber()))
                   .RuleFor(a => a.Street, f => f.Address.StreetName())
                   .RuleFor(a => a.City, f => f.Address.City())
                   .RuleFor(a => a.Country, f => f.Address.Country())
                   .RuleFor(a => a.PostalCode, f => int.Parse(f.Address.ZipCode().Where(char.IsDigit).ToArray()))
                   .RuleFor(a => a.CreatedAt, DateTime.UtcNow)
                   .RuleFor(a => a.UpdatedAt, DateTime.UtcNow);

                var addresses = addressFaker.Generate(15); // Generate 15 fake addresses

                context.Addresses.AddRange(addresses);
                context.SaveChanges();
            }

            //Seed Shops
            if (!context.Shops.Any())
            {
                var shopFaker = new Faker<Shop>("fr")
                    .RuleFor(s => s.ShopName, f => f.Company.CompanyName())
                    .RuleFor(s => s.ShopDescription, f => f.Lorem.Sentence())
                    .RuleFor(s => s.OwnerId, f => f.Random.Guid().ToString())
                    .RuleFor(s => s.CreatedAt, DateTime.UtcNow)
                    .RuleFor(s => s.UpdatedAt, DateTime.UtcNow);

                var shops = shopFaker.Generate(4); // Generate 5 fake shops

                context.Shops.AddRange(shops);
                context.SaveChanges();
            }
            //Seed product type
            if (!context.ProductTypes.Any())
            {
                var productTypeFaker = new Faker<ProductType>("fr")
                    .RuleFor(pt => pt.Name, f => f.Commerce.ProductName())
                    .RuleFor(pt => pt.Description, f => f.Lorem.Sentence())
                    .RuleFor(pt => pt.CreatedAt, DateTime.UtcNow)
                    .RuleFor(pt => pt.UpdatedAt, DateTime.UtcNow);

                var productTypes = productTypeFaker.Generate(5); // Generate 5 fake product types

                context.ProductTypes.AddRange(productTypes);
                context.SaveChanges();
            }

            // Seed Products
            //Every shop will have 10 products and the productType will be randomly selected 
            if (!context.Products.Any())
            {
                var shops = context.Shops.ToList();
                var productTypes = context.ProductTypes.ToList();

                var productFaker = new Faker<Product>("fr")
                    .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                    .RuleFor(p => p.Stock, f => f.Random.Number(1, 100))
                    .RuleFor(p => p.Price, f => f.Random.Double(25, 1500))
                    .RuleFor(p => p.Description, f => f.Lorem.Sentence())
                    .RuleFor(p => p.CreatedAt, DateTime.UtcNow)
                    .RuleFor(p => p.UpdatedAt, DateTime.UtcNow);

                foreach (var shop in shops)
                {
                    var productsForShop = productFaker
                    .RuleFor(p => p.ShopId, shop.ShopId)
                    .RuleFor(p => p.ProductTypeId, f => f.PickRandom(productTypes.Select(pt => pt.Id)))
                    .Generate(10);

                    context.Products.AddRange(productsForShop);
                }

                context.SaveChanges();
            }

            // Seed Reductions
            if (!context.Reductions.Any())
            {
                var shops = context.Shops.ToList();

                var reductionFaker = new Faker<Reduction>("fr")
                    .RuleFor(r => r.Title, f => f.Lorem.Word())
                    .RuleFor(r => r.Description, f => f.Lorem.Sentence())
                    .RuleFor(r => r.Percentage, f => f.Random.Double(1, 75))
                    .RuleFor(r => r.BeginDate, f => f.Date.Past().ToUniversalTime())
                    .RuleFor(r => r.EndDate, f => f.Date.Future().ToUniversalTime())
                    .RuleFor(r => r.CreatedAt, DateTime.UtcNow)
                    .RuleFor(r => r.UpdatedAt, DateTime.UtcNow);

                foreach (var shop in shops)
                {
                    var reductionsForShop = reductionFaker
                        .RuleFor(r => r.ShopId, shop.ShopId)
                        .Generate(3);

                    context.Reductions.AddRange(reductionsForShop);
                }
                context.SaveChanges();
            }

            // Seed ProductReductions
            if (!context.ProductReductions.Any())
            {
                var shops = context.Shops.ToList();

                var productReductionFaker = new Faker<ProductReduction>("fr")
                    // .RuleFor(pr => pr.ProductId, (f, pr) => f.PickRandom(context.Products.Where(p => p.ShopId == pr.Product.ShopId).Select(p => p.ProductId)))
                    // .RuleFor(pr => pr.ReductionId, f => f.PickRandom(context.Reductions.Select(r => r.Id)))
                    .RuleFor(pr => pr.IsActivated, f => f.Random.Bool())
                    .RuleFor(pr => pr.CreatedAt, DateTime.UtcNow)
                    .RuleFor(pr => pr.UpdatedAt, DateTime.UtcNow);

                foreach (var shop in shops)
                {
                    var product = context.Products.FirstOrDefault(p => p.ShopId == shop.ShopId);
                    var reduction = context.Reductions.FirstOrDefault(p => p.ShopId == shop.ShopId);
                    var productReductionForShop = productReductionFaker
                        .RuleFor(pr => pr.ProductId, product!.ProductId)
                        .RuleFor(pr => pr.ReductionId, reduction!.Id)
                        .Generate(1); // Generate 1 product reduction for each shop

                    context.ProductReductions.AddRange(productReductionForShop);
                }

                context.SaveChanges();
            }

        }
    }
}

