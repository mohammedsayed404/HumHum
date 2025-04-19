namespace Shared;

public record ProductParameterRequest
    (int? RestaurantId, int? CategoryId)
{
    private string? search;

    public string? Search
    {
        get { return search; }
        init { search = value?.ToLower().Trim(); }
    }
}
