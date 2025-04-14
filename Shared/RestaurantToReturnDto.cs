namespace Shared;

public record RestaurantToReturnDto(int Id, string Name)
{
    public string Image { get; init; } = string.Empty;
}
