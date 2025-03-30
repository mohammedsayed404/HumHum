namespace HumHum.ViewModels;

public record ProductToCreationViewModels(string Name, string Description, decimal Price,
    IFormFile Image, int RestaurantId, int CategoryId);

