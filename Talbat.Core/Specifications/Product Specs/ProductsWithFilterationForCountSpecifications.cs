using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core.Entities;

namespace Talbat.Core.Specifications.Product_Specs
{
    public class ProductsWithFilterationForCountSpecifications : BaseSpecifications<Product>
    {
        public ProductsWithFilterationForCountSpecifications(ProductSpecificationsParameters sepcParam) 
            : base(P =>
            (string.IsNullOrEmpty(sepcParam.Search) || P.Name.ToLower().Contains(sepcParam.Search))
                        &&
                    (!sepcParam.brandId.HasValue || P.BrandId == sepcParam.brandId.Value)
                        &&
                    (!sepcParam.categoryId.HasValue || P.CategoryId == sepcParam.categoryId.Value)
            )
        {
            
        }
    }
}
