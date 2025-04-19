using Domain.Contracts;
using Domain.Entities.Aggregates;

namespace Services.Specifications;

internal sealed class OrderWithItemsAndDeliveryMethod : SpecificationsBase<Order>
{


    public OrderWithItemsAndDeliveryMethod(Guid orderId)
        : base(order => order.Id == orderId)
    {
        AddIncludes(order => order.OrderItems);
        AddIncludes(order => order.DeliveryMethod);
    }

    public OrderWithItemsAndDeliveryMethod(string userEmail)
        : base(order => order.UserEmail == userEmail)
    {
        AddIncludes(order => order.OrderItems);
        AddIncludes(order => order.DeliveryMethod);
    }



}
