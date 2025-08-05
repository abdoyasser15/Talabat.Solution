using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core;
using Talbat.Core.Entities;
using Talbat.Core.Services.Contract;
using Talbat.Core.Specifications.Product_Specs;

namespace Talbat.Service
{
    public class ProductService : IProductService
    {
        private readonly IUntiOfWork _untiOfWork;

        public ProductService(IUntiOfWork untiOfWork)
        {
            _untiOfWork = untiOfWork;
        }
        public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecificationsParameters specParam)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(specParam);
            var Products = await _untiOfWork.Repository<Product>().GetAllSpecificationsAsync(spec);
            return Products;
        }
        public async Task<Product?> GetProductByIdAsync(int ProductId)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(ProductId);
            var product = await _untiOfWork.Repository<Product>().GetWithSpecAsync(spec);
            return product;
        }
        public async Task<int> GetCountAsync(ProductSpecificationsParameters specParam)
        {
            var countSpec = new ProductsWithFilterationForCountSpecifications(specParam);
            var count = await _untiOfWork.Repository<Product>().GetCountAsync(countSpec);
            return count;
        }
        public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
            => await _untiOfWork.Repository<ProductBrand>().GetAllAsync();

        public Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync()
            => _untiOfWork.Repository<ProductCategory>().GetAllAsync();
        
    }
}
