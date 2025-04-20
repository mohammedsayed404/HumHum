using Microsoft.AspNetCore.Http;

namespace Shared.ViewModels;

public record ProductCategoryToCreationViewModel(string Name, IFormFile Image);
