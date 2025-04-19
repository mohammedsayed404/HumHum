namespace Shared;

public record ProductToRestaurantWithQuantityViewModel()
{
    public List<ProductToReturnDto> Products { get; init; }
    public string RestaurantName { get; init; }

    public List<int> Quantity { get; init; } = default!;
}

