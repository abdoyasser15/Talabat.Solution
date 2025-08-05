using AutoMapper;
using AutoMapper.Execution;
using Talabat.APIs.Dtos;
using Talbat.Core.Entities;
using Talbat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.Helpers
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItems, OrderItemsDto, string>
    {
        private readonly IConfiguration configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string Resolve(OrderItems source, OrderItemsDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ItemOrdered.PictureUrl))
                return $"{configuration["ApiBaseUrl"]}/{source.ItemOrdered.PictureUrl}";

            return string.Empty;
        }
        
    }
}
