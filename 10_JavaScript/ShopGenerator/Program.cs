using Bogus;               // Nuget: dotnet add package Bogus --version 34.*
using System.Text.Json;
Randomizer.Seed = new Random(1846);

var stores = new Faker<Store>("de").CustomInstantiator(f => new Store(
        f.Random.Guid(),
        f.Company.CompanyName(),
        f.Address.StreetAddress(),
        (f.Random.Int(101,123) * 10).ToString(),
        "Wien"
    )).Generate(5)
    .GroupBy(s => s.Name).Select(s => s.First())
    .ToList();

var productCategories = new Faker<ProductCategory>("de").CustomInstantiator(f => new ProductCategory(
        f.Random.Guid(),
        f.Commerce.ProductAdjective()
    )).Generate(5)
    .GroupBy(s => s.Name).Select(s => s.First())
    .ToList();

var products = new Faker<Product>("de").CustomInstantiator(f => new Product(
        f.Random.Guid(),
        f.Random.Int(100000, 999999),
        f.Commerce.ProductName(),
        f.Commerce.ProductDescription(),
        new DateTime(2021, 1, 1).AddSeconds(f.Random.Int(0, 10 * 30 * 86400)),
        f.Random.ListItem(productCategories)
    )).Generate(25)
    .GroupBy(s => s.Name).Select(s => s.First())
    .ToList();

var offers = new Faker<Offer>("de").CustomInstantiator(f => 
    {
        var product = f.Random.ListItem(products);
        return new Offer(
            f.Random.Guid(),
            product,
            f.Random.ListItem(stores),
            Math.Round(f.Random.Decimal(100, 1000), 2),
            product.Added.AddSeconds(f.Random.Int(86400, 2 * 30 * 86400))
        );
    }).Generate(125)
    .GroupBy(o => new { SGuid = o.Store.Guid, PGuid = o.Product.Guid }).Select(s => s.First())
    .ToList();

string fileName = "Stores.json";
string jsonString = JsonSerializer.Serialize(new
{
    Stores = stores,
    ProductCategories = productCategories,
    Products = products,
    Offers = offers
}, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true });
File.WriteAllText(fileName, jsonString, new System.Text.UTF8Encoding(false));

public record Store(Guid Guid, string Name, string Address, string Zip, string City);
public record ProductCategory(Guid Guid, string Name);
public record Product(Guid Guid, int Ean, string Name, string Description, DateTime Added, ProductCategory ProductCategory);
public record Offer(Guid Guid, Product Product, Store Store, decimal Price, DateTime LastUpdate);

