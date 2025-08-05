using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Talbat.Core.Entities.Order_Aggregate
{
    public class Order : BaseEntity
    {
        

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Address ShippingAddress { get; set; }
        //public int DeliveryMethodId { get; set; } // Foreign Key DeliveryMethod
        public DeliveryMethod DeliveryMethod { get; set; } // Navigation property for delivery method [O]
        public ICollection<OrderItems> Items { get; set; } = new HashSet<OrderItems>(); // Navagation property for order items [Many]
        public decimal Subtotal { get; set; }
        public decimal GetTotal() 
            =>  Subtotal + DeliveryMethod.Cost;
        public string PaymentIntentId { get; set; } // Used for payment processing
        // Here Accessible Empty Parameterless Constructor must Be Exist
        public Order()
        {
            
        }
        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod
            , ICollection<OrderItems> items, decimal subtotal , string PaymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            Subtotal = subtotal;
            this.PaymentIntentId = PaymentIntentId;  
        }
    }
}
