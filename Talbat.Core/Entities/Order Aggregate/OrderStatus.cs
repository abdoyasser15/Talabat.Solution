using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Talbat.Core.Entities.Order_Aggregate
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending, // Order is created but not yet processed

        [EnumMember(Value = "Payment Received")]
        PaymentReceived, // Payment has been successfully processed

        [EnumMember(Value = "Payment Failed")]
        PaymentFailed // Payment processing failed
    }
}
