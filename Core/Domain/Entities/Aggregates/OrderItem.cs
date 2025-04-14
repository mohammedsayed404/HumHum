using Domain.Common;

namespace Domain.Entities.Aggregates;

public class OrderItem : EntityBase<Guid>
{

    public ProductInOrderItem Product { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }


    public Order Order { get; set; } = null!;

}