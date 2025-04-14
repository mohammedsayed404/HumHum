namespace Shared;

public record CartItemDto
    (int Id, string ProductName, string ImageUrl,
    decimal Price, int Quantity, string Category, string Restaurant);
