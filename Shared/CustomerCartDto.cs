namespace Shared;

public record CustomerCartDto(string Id, IReadOnlyList<CartItemDto> Items);



//public record CustomerCartDto(string Id, IReadOnlyList<CartItemDto> Items, int? DeliveryMethodId,
//    decimal DeliveryPrice, string? PaymentIntentId, string? ClientSecret);