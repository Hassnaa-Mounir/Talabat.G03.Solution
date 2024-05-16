using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.CoreLayer.Entities;

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

        }
    }
}
