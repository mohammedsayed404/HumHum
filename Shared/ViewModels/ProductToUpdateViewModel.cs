using Microsoft.AspNetCore.Http;

namespace Shared.ViewModels;

public record ProductToUpdateViewModel(int Id, string Name, string Description, decimal Price,
     int RestaurantId, int CategoryId, string? ImageUrl, string? PublicImageId)
{
    public IFormFile? Image { get; init; }
}

