using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core.Entities;
using Talbat.Core.Specifications.Product_Specs;

namespace Talbat.Core.Services.Contract
{
    public interface IProductService
    {
        Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecificationsParameters specParam);
        Task<Product?> GetProductByIdAsync(int ProductId);
        Task<IReadOnlyList<ProductBrand>> GetBrandsAsync();
        Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync();

        Task<int> GetCountAsync(ProductSpecificationsParameters specParam);

    }
}
