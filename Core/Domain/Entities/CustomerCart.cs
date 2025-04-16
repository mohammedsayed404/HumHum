namespace Domain.Entities;

public class CustomerCart
{
    public string Id { get; set; } = string.Empty;

    public IReadOnlyList<CartItem> Items { get; set; } = new List<CartItem>();

    public int? DeliveryMethodId { get; set; }
    public decimal DeliveryPrice { get; set; }
    public string? PaymentIntentId { get; set; } = string.Empty;

    public string? ClientSecret { get; set; } = string.Empty;

}
