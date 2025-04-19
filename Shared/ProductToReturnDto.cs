namespace Shared;

public record ProductToReturnDto(int Id, string Name, string Description, decimal Price,
    float Rate, string Restaurant, string Category)
{

    public string Image { get; init; } = string.Empty;
}

