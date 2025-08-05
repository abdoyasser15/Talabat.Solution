
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core.Entities.Order_Aggregate;

namespace Talbat.Core.Specifications.Orders_Specs
{
    public class OrderWithPaymentIntentSpecification : BaseSpecifications<Order>
    {
        public OrderWithPaymentIntentSpecification(string PaymentIntentId)
            :base(O => O.PaymentIntentId == PaymentIntentId)
        {
            
        }
    }
}
