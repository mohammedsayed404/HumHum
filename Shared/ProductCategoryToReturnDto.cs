namespace Shared;

public record ProductCategoryToReturnDto(string Name)
{
    public string Image { get; init; } = string.Empty;
}
