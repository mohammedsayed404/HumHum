using Domain.Entities;

namespace Domain.Contracts;

public interface ICartRepository
{

    Task<CustomerCart?> GetCartAsync(string id);

    Task<CustomerCart?> CreateOrUpdateCartAsync(CustomerCart cart);

    Task<bool> DeleteCartAsync(string id);

}
