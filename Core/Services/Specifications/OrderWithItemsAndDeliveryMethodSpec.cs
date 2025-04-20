using Domain.Contracts;
using Domain.Entities.Aggregates;

namespace Services.Specifications;

internal sealed class OrderWithItemsAndDeliveryMethodSpec : SpecificationsBase<Order>
{


    public OrderWithItemsAndDeliveryMethodSpec(Guid orderId)
        : base(order => order.Id == orderId)
    {
        AddIncludes(order => order.OrderItems);
        AddIncludes(order => order.DeliveryMethod);
    }

    //public OrderWithItemsAndDeliveryMethod(string userEmail)

    public OrderWithItemsAndDeliveryMethodSpec(string userEmail)
        : base(order => order.UserEmail == userEmail)
    {
        AddIncludes(order => order.OrderItems);
        AddIncludes(order => order.DeliveryMethod);
    }



}
