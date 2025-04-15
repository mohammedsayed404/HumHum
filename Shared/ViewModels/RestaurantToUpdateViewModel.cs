using Microsoft.AspNetCore.Http;

namespace Shared.ViewModels;

public record RestaurantToUpdateViewModel(int Id, string Name, string? ImageUrl, string? PublicImageId)
{
    public IFormFile? Image { get; init; }
}
