namespace Shared;

public record ApplicationUserToReturnDto
    (string Id, string DisplayName, string Email, string PhoneNumber, AddressToReturnDto Address);
