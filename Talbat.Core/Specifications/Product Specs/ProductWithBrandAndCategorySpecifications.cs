using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core.Entities;

namespace Talbat.Core.Specifications.Product_Specs
{
    public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
    {
        // This Constructor Will Be Used For Creating Object , That Will Be Used To Get All Products With Brand And Category
        public ProductWithBrandAndCategorySpecifications(ProductSpecificationsParameters specParam)
            :base(P=>
                (string.IsNullOrEmpty(specParam.Search) || P.Name.ToLower().Contains(specParam.Search))
                        &&
                    (!specParam.brandId.HasValue || P.BrandId == specParam.brandId.Value)
                        &&
                    (!specParam.categoryId.HasValue || P.CategoryId== specParam.categoryId.Value)
            )
        {
            AddIncludes();
            if (!string.IsNullOrEmpty(specParam.sort))
            {
                switch (specParam.sort)
                {
                    case "priceAsc":
                        //OrderBy = p => p.Price;
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        //OrderByDesc = p => p.Price;
                        AddOrderByDesc(p => p.Price);
                        break;
                    default:
                        //OrderBy = p => p.Name; // Default sorting by Name
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(p => p.Name); // Default sorting by Name if no sort parameter is provided
            }

            ApplyPagination((specParam.pageIndex - 1) * specParam.PageSize, specParam.PageSize);
        }
        // This Constructor Will Be Used For Creating Object, That Will Be Used To Get Specific Product By Id With Brand And Category
        public ProductWithBrandAndCategorySpecifications(int id)
            : base(P=>P.Id==id)
        {
            AddIncludes();
        }
        private void AddIncludes()
        {
            Include.Add(p => p.Brand);
            Include.Add(p => p.Category);
        }
    }
}
