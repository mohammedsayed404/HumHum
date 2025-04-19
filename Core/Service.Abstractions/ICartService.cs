using Shared;

namespace Service.Abstractions;

public interface ICartService
{

    // ======== Create , Update , Delete ======== //
    /// <summary>
    /// Returns customer cart, 
    /// </summary>
    /// <param name="cartId">
    /// takes string cartId
    /// </param>
    /// <returns>
    /// returns customer cart, and if it doesn't exist throw an exception: cart not found
    /// </returns>
    Task<CustomerCartDto> GetCustomerCartAsync(string cartId);

    Task<CustomerCartDto> UpdateCustomerCartAsync(CustomerCartDto cart);

    Task<bool> DeleteCustomerCartAsync(string cartId);

}
