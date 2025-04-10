using AutoMapper;
using Domain.Entities;
using Shared;

namespace Services.MappingProfiles;

internal sealed class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<CustomerCart, CustomerCartDto>().ReverseMap();
        CreateMap<CartItem, CartItemDto>().ReverseMap();
    }


}
