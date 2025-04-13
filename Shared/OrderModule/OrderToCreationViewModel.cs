namespace Shared.OrderModule;

public record OrderToCreationViewModel(string CartId, int DeliveryMethodId, OrderAddressToReturnDto ShippingAddress);
