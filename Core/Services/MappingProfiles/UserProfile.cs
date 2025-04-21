using AutoMapper;
using Domain.Entities.Identity;
using Shared;

namespace Services.MappingProfiles;

internal sealed class UserProfile : Profile
{

    public UserProfile()
    {
        CreateMap<ApplicationUser, ApplicationUserToReturnDto>();
    }


}
