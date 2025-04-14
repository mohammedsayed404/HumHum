using Shared.OrderModule;

namespace Service.Abstractions;

public interface IOrderService
{
    //create order , getOrdersForUser , GetOrderFor Specific User , 


    //to get specific order to user 
    Task<OrderToReturnDto> GetOrderForUserByIdAsync(Guid id);

    //to show to user all orders list that he made
    Task<IReadOnlyList<OrderToReturnDto>> GetOrdersForUserByEmailAsync(string userEmail);

    //create order to user 
    Task<OrderToReturnDto> CreateOrderAsync(OrderToCreationViewModel model, string userEmail);

    //get all delivery method to make user chooses which one he prefer 
    Task<IReadOnlyList<DeliveryMethodToReturnDto>> GetAllDeliveryMethodsAsync();


}
