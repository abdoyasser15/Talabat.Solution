using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talbat.Core.Entities;
using Talbat.Core.Entities.Order_Aggregate;

namespace Talbat.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext _dbContext)
        {
            if (_dbContext.ProductBrands.Count()==0)
            {
                var brandsData = File.ReadAllText("../Talbat.Repository/Data/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                if (brands?.Count() > 0)
                {
                    foreach (var item in brands)
                    {
                        _dbContext.ProductBrands.Add(item);
                    }
                    await _dbContext.SaveChangesAsync();
                } 
            }
            if (_dbContext.ProductCategories.Count() == 0)
            {
                var CategoryData = File.ReadAllText("../Talbat.Repository/Data/DataSeed/categories.json");
                var Categories = JsonSerializer.Deserialize<List<ProductCategory>>(CategoryData);
                if (Categories?.Count() > 0)
                {
                    foreach (var item in Categories)
                    {
                        _dbContext.ProductCategories.Add(item);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            if (_dbContext.Products.Count() == 0)
            {
                var ProductData = File.ReadAllText("../Talbat.Repository/Data/DataSeed/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductData);
                if (Products?.Count() > 0)
                {
                    foreach (var item in Products)
                    {
                        _dbContext.Products.Add(item);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            if (_dbContext.DeliveryMehod.Count() == 0)
            {
                var deliveryMehodData = File.ReadAllText("../Talbat.Repository/Data/DataSeed/delivery.json");
                var deliveryMehod = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMehodData);
                if (deliveryMehod?.Count() > 0)
                {
                    foreach (var item in deliveryMehod)
                    {
                        _dbContext.DeliveryMehod.Add(item);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
