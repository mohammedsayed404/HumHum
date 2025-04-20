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

    public int pageNumber { set; get; } = 1;
    public int pageSize { set; get; } = 6;
}
