//using Domain.Entities;

using Shared;

namespace Service.Abstractions;

public interface IPaymentService
{
    //Create , Update
    
    Task<CustomerCartDto?> CreateOrUpdatePaymentIntent(string CartId);

}
