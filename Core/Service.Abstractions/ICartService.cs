using Shared;

namespace Service.Abstractions;

public interface ICartService
{

    // ======== Create , Update , Delete ======== //

    Task<CustomerCartDto> GetCustomerCartAsync(string cartId);

    Task<CustomerCartDto> UpdateCustomerCartAsync(CustomerCartDto cart);

    Task<bool> DeleteCustomerCartAsync(string cartId);

}
