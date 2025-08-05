using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core.Entities.Order_Aggregate;

namespace Talbat.Core.Specifications.Orders_Specs
{
    public class OrderSpecifications : BaseSpecifications<Order>
    {
        public OrderSpecifications(string buyerEmail)
            : base(O=>O.BuyerEmail == buyerEmail)
        {
            Include.Add(O => O.DeliveryMethod);
            Include.Add(O => O.Items);

            AddOrderByDesc(O => O.OrderDate);
        }
        public OrderSpecifications(int id, string buyerEmail)
            : base(O => O.Id == id && O.BuyerEmail == buyerEmail)
        {
            Include.Add(O => O.DeliveryMethod);
            Include.Add(O => O.Items);
        }
        
    }
}
