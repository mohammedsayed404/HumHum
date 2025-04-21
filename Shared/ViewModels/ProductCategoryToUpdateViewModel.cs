using Microsoft.AspNetCore.Http;

namespace Shared.ViewModels;

public record ProductCategoryToUpdateViewModel(int Id, string Name, string? ImageUrl, string? PublicImageId)
{
    public IFormFile? Image { get; init; }
}
