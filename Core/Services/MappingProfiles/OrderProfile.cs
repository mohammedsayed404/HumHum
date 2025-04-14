using AutoMapper;
using Domain.Entities.Aggregates;
using Shared.OrderModule;

namespace Services.MappingProfiles;

internal sealed class OrderProfile : Profile
{

    public OrderProfile()
    {

        CreateMap<OrderAddress, OrderAddressToReturnDto>().ReverseMap();

        CreateMap<OrderItem, OrderItemToReturnDto>()
            .ForCtorParam(nameof(OrderItemToReturnDto.ProductId), options => options.MapFrom(source => source.Product.Id))
            .ForCtorParam(nameof(OrderItemToReturnDto.ProductName), options => options.MapFrom(source => source.Product.Name))
            .ForMember(dest => dest.ProductImage,
              options =>
              options.MapFrom<PhotoResolver<OrderItem, OrderItemToReturnDto>>());


        CreateMap<Order, OrderToReturnDto>()
            .ForCtorParam(nameof(OrderToReturnDto.PaymentStatus), options => options.MapFrom(source => source.PaymentStatus.ToString()))
            .ForCtorParam(nameof(OrderToReturnDto.DeliveryMethod), options => options.MapFrom(source => source.DeliveryMethod.ShortName));



    }


}
