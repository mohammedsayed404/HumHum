using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Service.Abstractions;
using Shared;

namespace Services;

internal sealed class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    public CartService(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    public async Task<bool> DeleteCustomerCartAsync(string cartId)
        => await _cartRepository.DeleteCartAsync(cartId);

    public async Task<CustomerCartDto> GetCustomerCartAsync(string cartId)
    {
        var customerCart = await _cartRepository.GetCartAsync(cartId);

        return customerCart is not null ? _mapper.Map<CustomerCartDto>(customerCart) :
            throw new CartNotFoundException(cartId);

    }

    public async Task<CustomerCartDto> UpdateCustomerCartAsync(CustomerCartDto cart)
    {
        var customerCart = _mapper.Map<CustomerCart>(cart);

        var createdOrUpdated = await _cartRepository.CreateOrUpdateCartAsync(customerCart);


        return createdOrUpdated is not null ? _mapper.Map<CustomerCartDto>(createdOrUpdated) :
            throw new UpdateCartException(cart?.Id!);
    }
}
