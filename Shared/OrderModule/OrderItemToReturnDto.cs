namespace Shared.OrderModule;

public record OrderItemToReturnDto
    (Guid Id, int ProductId, string ProductName, int Quantity, decimal Price)
{
    public string ProductImage { get; init; } = string.Empty;
}

