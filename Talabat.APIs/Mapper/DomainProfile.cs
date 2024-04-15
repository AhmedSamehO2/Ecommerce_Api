using AutoMapper;
using Talabat.APIs.DTOS;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.OrderAggregat;


namespace Talabat.APIs.Mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Product, ProductToReturnDto>()
             .ForMember(d => d.ProductType, o => o.MapFrom(S => S.ProductType.Name))
             .ForMember(d => d.ProductBrand, o => o.MapFrom(S => S.ProductBrand.Name))
             .ForMember(d=>d.PictureUrl,o=>o.MapFrom<ProductPictureUrlResolver>());

            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();
            CreateMap<AddressDto, OrderAddress>();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d=>d.DeliveryMethod,o=>o.MapFrom(S=>S.DeliveryMethod.ShortName))
                .ForMember(d=>d.DeliveryMethodCost,o=>o.MapFrom(S=>S.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(S => S.Product.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(S => S.Product.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(S => S.Product.PictureUrl))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderPictureUrlResolver>());




        }
    }
}
