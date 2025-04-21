namespace Shared;

public record ProductCategoryToReturnDto(int Id, string Name)
{
    public string Image { get; init; } = string.Empty;
}
