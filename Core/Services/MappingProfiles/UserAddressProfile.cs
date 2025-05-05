using AutoMapper;
using Domain.Entities.Identity;
using Shared;
using Shared.ViewModels;

namespace Services.MappingProfiles;

internal sealed class UserAddressProfile : Profile
{
    public UserAddressProfile()
    {
        CreateMap<Address, AddressToReturnDto>().ReverseMap();
        CreateMap<AddressToUpdateViewModel, Address>();
        CreateMap<AddressToReturnDto, AddressToUpdateViewModel>();
    }

}
