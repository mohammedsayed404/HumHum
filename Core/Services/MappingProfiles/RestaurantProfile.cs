using AutoMapper;
using Domain.Entities;
using Shared;
using Shared.ViewModels;

namespace Services.MappingProfiles;

internal sealed class RestaurantProfile : Profile
{
    public RestaurantProfile()
    {
        CreateMap<Restaurant, RestaurantToReturnDto>()
            .ForMember(dest => dest.Image,
            options =>
            options.MapFrom<PhotoResolver<Restaurant, RestaurantToReturnDto>>());


        CreateMap<RestaurantToCreationViewModel, Restaurant>();

        CreateMap<RestaurantToUpdateViewModel, Restaurant>().ReverseMap();
    }
}
