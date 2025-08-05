using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Talbat.Core.Entities.Order_Aggregate
{
    public class OrderItems : BaseEntity
    {
        
        public ProductItemOrdered ItemOrdered { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        [JsonIgnore]
        public Order Order { get; set; }
        public OrderItems()
        {
            
        }
        public OrderItems(ProductItemOrdered product, decimal price, int quantity)
        {
            ItemOrdered = product;
            Price = price;
            Quantity = quantity;
        }
    }
}
