using AutoMapper;
using Domain.Entities.Identity;
using Shared;
using Shared.ViewModels;

namespace Services.MappingProfiles;

internal sealed class UserProfile : Profile
{

    public UserProfile()
    {
        CreateMap<ApplicationUser, ApplicationUserToReturnDto>().ReverseMap();
        CreateMap<ApplicationUserToUpdateViewModule, ApplicationUser>();
        CreateMap<ApplicationUserToReturnDto, ApplicationUserToUpdateViewModule>();
    }


}
