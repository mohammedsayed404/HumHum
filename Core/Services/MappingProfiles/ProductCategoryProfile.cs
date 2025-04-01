using AutoMapper;
using Domain.Entities;
using Shared;

namespace Services.MappingProfiles;

internal sealed class ProductCategoryProfile : Profile
{
    public ProductCategoryProfile()
    {
        CreateMap<ProductCategory, ProductCategoryToReturnDto>()
            .ForMember(dest => dest.Image,
            options =>
            options.MapFrom<PhotoResolver<ProductCategory, ProductCategoryToReturnDto>>());
    }
}
