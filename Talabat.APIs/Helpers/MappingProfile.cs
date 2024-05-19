using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.CoreLayer.Entities;
using Talabat.CoreLayer.Entities.Idintity;
using OrderAddress = Talabat.CoreLayer.Entities.Order_Aggregate;


namespace Talabat.APIs.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(P => P.Brand, option => option.MapFrom(S => S.ProductBrand.Name))
                .ForMember(P => P.Category, option => option.MapFrom(S => S.ProductType.Name))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<ProductPictureUrlResolver>());
            //.ForMember(P => P.PictureUrl, option => option.MapFrom(S => $"{_configuration["ApiBaseUrl"]}/{S.PictureUrl}"));

            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
           // CreateMap<Address, AddressDto>();
            CreateMap<Address, AddressDto>().ReverseMap();
           // CreateMap<AddressDto, OrderAddress.Address>();
            CreateMap<AddressDto, OrderAddress.Address>();

            CreateMap<OrderAddress.Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));

            CreateMap<OrderAddress.OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl));

        }
    }
}
