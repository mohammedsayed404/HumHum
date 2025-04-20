using AutoMapper;
using Domain.Entities;
using Shared;
using Shared.ViewModels;

namespace Services.MappingProfiles;

internal sealed class ProductCategoryProfile : Profile
{
    public ProductCategoryProfile()
    {
        CreateMap<ProductCategory, ProductCategoryToReturnDto>()
            .ForMember(dest => dest.Image,
            options =>
            options.MapFrom<PhotoResolver<ProductCategory, ProductCategoryToReturnDto>>());

        CreateMap<ProductCategoryToCreationViewModel, ProductCategory>();

        CreateMap<ProductCategoryToUpdateViewModel, ProductCategory>().ReverseMap();
    }
}
