namespace Shared.ViewModels;

public record ProductWithRestaurantToReturnDto(int Id, string Name, string Description, decimal Price,
    float Rate, RestaurantToReturnDto Restaurant, string Category)
{
    public string ProductImage { get; init; } = string.Empty;
    
}
