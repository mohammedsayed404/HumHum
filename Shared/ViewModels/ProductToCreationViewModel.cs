using Microsoft.AspNetCore.Http;

namespace Shared.ViewModels;

public record ProductToCreationViewModel(string Name, string Description, decimal Price,
    IFormFile Image, int RestaurantId, int CategoryId);




//public record ProductToCreationViewModel(string Name, string Description,
//    IFormFile Image, int RestaurantId, int CategoryId)
//{

//    [Range(1, int.MaxValue)]
//    public decimal Price { get; set; }

//}


