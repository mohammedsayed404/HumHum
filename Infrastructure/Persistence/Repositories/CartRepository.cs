using Domain.Contracts;
using Domain.Entities;
using StackExchange.Redis;
using System.Text.Json;

namespace Persistence.Repositories;

public sealed class CartRepository : ICartRepository
{
    private readonly IDatabase _database;

    public CartRepository(IConnectionMultiplexer ConnectionMultiplexer)
    {
        _database = ConnectionMultiplexer.GetDatabase();
    }


    public async Task<CustomerCart?> CreateOrUpdateCartAsync(CustomerCart cart)
    {

        var json = JsonSerializer.Serialize(cart);

        var isCreatedOrUpdated = await _database.StringSetAsync(cart.Id, json, TimeSpan.FromDays(10));

        return isCreatedOrUpdated ? await GetCartAsync(cart.Id) : null;

    }

    public async Task<bool> DeleteCartAsync(string id)
        => await _database.KeyDeleteAsync(id);

    public async Task<CustomerCart?> GetCartAsync(string id)
    {
        var cart = await _database.StringGetAsync(id);

        return cart.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerCart>(cart!);
    }
}
