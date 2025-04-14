namespace Domain.Entities;

public class CustomerCart
{
    public string Id { get; set; } = string.Empty;

    public IReadOnlyList<CartItem> Items { get; set; } = new List<CartItem>();


}
