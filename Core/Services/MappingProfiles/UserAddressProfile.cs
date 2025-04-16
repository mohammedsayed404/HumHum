using AutoMapper;
using Domain.Entities.Identity;
using Shared;
using Shared.ViewModels;

namespace Services.MappingProfiles;

internal sealed class UserAddressProfile : Profile
{
    public UserAddressProfile()
    {
        CreateMap<Address, AddressToReturnDto>();
        CreateMap<AddressToUpdateViewModel, Address>();
    }

}
