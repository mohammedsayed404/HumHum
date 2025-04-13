namespace Shared.OrderModule;

public record DeliveryMethodToReturnDto
    (int Id, string ShortName, string Description, string DeliveryTime, decimal Price);