using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregat;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext dbContext)
        {
            if(!dbContext.productBrands.Any())
            {
                var BrandsData = File.ReadAllText("../Talabat.Repository/Data\\DataSeed/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);
                if (Brands?.Count > 0)
                {
                    foreach (var Brand in Brands)
                        await dbContext.Set<ProductBrand>().AddAsync(Brand);
                    await dbContext.SaveChangesAsync();
                }
            }
            if (!dbContext.productTypes.Any())
            {
                var TypeData = File.ReadAllText("../Talabat.Repository/Data\\DataSeed/types.json");
                var Types = JsonSerializer.Deserialize<List<ProductType>>(TypeData);
                if (Types?.Count > 0)
                {
                    foreach (var type in Types)
                        await dbContext.Set<ProductType>().AddAsync(type);
                    await dbContext.SaveChangesAsync();
                }
            }
            if (!dbContext.products.Any())
            {
                var ProductData = File.ReadAllText("../Talabat.Repository/Data\\DataSeed/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductData);
                if (Products?.Count > 0)
                    foreach (var product in Products)
                        await dbContext.Set<Product>().AddAsync(product);
                await dbContext.SaveChangesAsync();
            }
            if (!dbContext.DeliveryMethods.Any())
            {
                var deliveryMethods = File.ReadAllText("../Talabat.Repository/Data\\DataSeed/delivery.json");
                var Delivery = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethods);
                if (Delivery?.Count > 0)
                    foreach (var method in Delivery)
                      await  dbContext.Set<DeliveryMethod>().AddAsync(method);
                      await dbContext.SaveChangesAsync();
            }
        }
    }
}
