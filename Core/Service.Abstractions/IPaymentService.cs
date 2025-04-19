using Shared;
using Shared.OrderModule;
using Stripe;

namespace Service.Abstractions;

public interface IPaymentService
{
    //Create , Update
    
    Task<CustomerCartDto?> CreateOrUpdatePaymentIntent(string CartId);
    //Task<Order>  UpdatePaymentIntentForSucceededOrFailed(string paymentIntent, bool flag);
    Task<OrderToReturnDto>  UpdatePaymentIntentForSucceededOrFailed(string paymentIntent, bool flag);


}
