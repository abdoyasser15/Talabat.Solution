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
    internal class OrderItemConfigration : IEntityTypeConfiguration<OrderItems>
    {
        public void Configure(EntityTypeBuilder<OrderItems> builder)
        {
            builder.OwnsOne(orderItem => orderItem.ItemOrdered, io =>
            {
                io.Property(p => p.ProductId).HasColumnName("ProductId");
                io.Property(p => p.Description).HasColumnName("Product_Description");
                io.Property(p => p.PictureUrl).HasColumnName("Product_PictureUrl");
            });

            builder.Property(orderItem => orderItem.Price)
                .HasColumnType("decimal(18,2)"); 
        }
    }
}
