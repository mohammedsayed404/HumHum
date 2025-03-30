using AutoMapper;
using Domain.Entities;
using Shared;

namespace Services.MappingProfiles;

internal sealed class ProductProfile : Profile
{
    public ProductProfile()
    {

        CreateMap<Product, ProductToReturnDto>()
            .ForMember(dest => dest.CategoryName, options => options.MapFrom(source => source.Category.Name))
            .ForMember(dest => dest.RestaurantName, options => options.MapFrom(source => source.Restaurant.Name));


    }

}
