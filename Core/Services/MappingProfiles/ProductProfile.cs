using AutoMapper;
using Domain.Entities;
using Shared.ViewModels;
using Shared;

namespace Services.MappingProfiles;

internal sealed class ProductProfile : Profile
{
    public ProductProfile()
    {

        CreateMap<Product, ProductToReturnDto>()
            .ForCtorParam(nameof(ProductToReturnDto.Category), options => options.MapFrom(source => source.Category.Name))
            .ForCtorParam(nameof(ProductToReturnDto.Restaurant), options => options.MapFrom(source => source.Restaurant.Name))
            .ForMember(dest => dest.Image,
              options =>
              options.MapFrom<PhotoResolver<Product, ProductToReturnDto>>());




        CreateMap<ProductToCreationViewModel, Product>();

        CreateMap<ProductToUpdateViewModel, Product>().ReverseMap();

        CreateMap<Product, ProductWithRestaurantToReturnDto>()
            .ForMember(dest => dest.ProductImage,
              options =>
              options.MapFrom<PhotoResolver<Product, ProductWithRestaurantToReturnDto>>()
            
           );

    }

}
