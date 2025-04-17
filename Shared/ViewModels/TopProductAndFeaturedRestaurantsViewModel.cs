namespace Shared.ViewModels;

public record TopProductAndFeaturedRestaurantsViewModel(
    IReadOnlyList<ProductToReturnDto> Products, IReadOnlyList<ProductWithRestaurantToReturnDto> ProductWithRestaurants);

