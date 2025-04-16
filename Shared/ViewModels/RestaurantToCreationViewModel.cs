using Microsoft.AspNetCore.Http;

namespace Shared.ViewModels;

public record RestaurantToCreationViewModel(string Name, IFormFile Image);
