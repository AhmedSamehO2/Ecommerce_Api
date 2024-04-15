using AutoMapper;
using Microsoft.Extensions.Configuration;
using Talabat.APIs.DTOS;
using Talabat.Core.Entities.OrderAggregat;

namespace Talabat.APIs.Mapper
{
    public class OrderPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;
        public OrderPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
            {
                return $"{_configuration["ApiBaseUrl"]}{source.Product.PictureUrl}";
            }
            return string.Empty;
        }
    }
}
