namespace Shared;

public record ProductToRestaurantWithQuantityViewModel()
{
    public required List<ProductToReturnDto> Products { get; init; }
    public required string RestaurantName { get; init; }

    public List<int> Quantity { get; init; } = default!;
    public int TotalPages { get; init; }
    public int CurrentPage { get; init; }
}

