namespace Shared.OrderModule;

public record OrderToReturnDto(Guid Id, string UserEmail,
    string PaymentStatus, string DeliveryMethod, decimal Subtotal, decimal Total,
    string PaymentIntentId, DateTimeOffset OrderDate, OrderAddressToReturnDto ShippingAddress,
    IReadOnlyList<OrderItemToReturnDto> OrderItems
    );

