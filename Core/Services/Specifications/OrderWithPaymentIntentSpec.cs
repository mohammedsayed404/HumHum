using Domain.Contracts;
using Domain.Entities.Aggregates;

namespace Services.Specifications;

internal sealed class OrderWithPaymentIntentSpec : SpecificationsBase<Order>
{
    public OrderWithPaymentIntentSpec()
    {
        
    }
    public OrderWithPaymentIntentSpec(string paymentIntentId)
    : base(O => O.PaymentIntentId == paymentIntentId)
    {

    }
}
