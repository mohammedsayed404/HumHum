using Domain.Common;
using Domain.Enums;

namespace Domain.Entities.Aggregates;

public class Order : EntityBase<Guid>
{

    public string UserEmail { get; set; } = string.Empty;
    public OrderAddress ShippingAddress { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();

    public OrderPaymentStatus PaymentStatus { get; set; } = OrderPaymentStatus.Pending;


    public int? DeliveryMethodId { get; set; }

    public DeliveryMethod DeliveryMethod { get; set; } = null!;


    // subTotal => items.Q * Price  
    public decimal Subtotal { get; set; }


    // Payment [i will do it soon]

    public string PaymentIntentId { get; set; } = string.Empty;

    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;


    public decimal Total => Subtotal + (DeliveryMethod?.Price ?? 0);
}


