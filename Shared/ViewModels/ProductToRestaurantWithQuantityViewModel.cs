namespace Shared;

public record ProductToRestaurantWithQuantityViewModel()
{
    public List<ProductToReturnDto> Products { get; set; }
    public string RestaurantName { get; set; }
    public int? RestaurantId { get; init; }
    public List<int> Quantity { get; set; } = default!;
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
}

