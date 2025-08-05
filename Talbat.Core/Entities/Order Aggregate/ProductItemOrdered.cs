using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talbat.Core.Entities.Order_Aggregate
{
    public class ProductItemOrdered
    {
        

        public int ProductId { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public ProductItemOrdered()
        {
            
        }
        public ProductItemOrdered(int productId, string description, string pictureUrl)
        {
            ProductId = productId;
            Description = description;
            PictureUrl = pictureUrl;
        }
    }
}
