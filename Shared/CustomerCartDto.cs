namespace Shared;

public record CustomerCartDto(string Id, IReadOnlyList<CartItemDto> Items);
