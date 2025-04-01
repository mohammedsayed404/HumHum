namespace Shared;

public record RestaurantToReturnDto(string Name)
{
    public string Image { get; init; } = string.Empty;
}
