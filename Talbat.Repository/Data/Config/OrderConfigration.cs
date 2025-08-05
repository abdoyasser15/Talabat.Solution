using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core.Entities.Order_Aggregate;

namespace Talbat.Repository.Data.Config
{
    internal class OrderConfigration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(O => O.ShippingAddress, ShippingAddress => ShippingAddress.WithOwner()); // 1.1 [Total]
            builder.Property(O => O.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v));

            builder.Property(O=>O.Subtotal).
                HasColumnType("decimal(18,2)");

            builder.HasOne(O=>O.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull); // Prevent cascade delete
        }
    }
}
