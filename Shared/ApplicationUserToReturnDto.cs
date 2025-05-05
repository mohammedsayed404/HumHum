namespace Shared;

public record ApplicationUserToReturnDto
    (string Id, string UserName, string Email, AddressToReturnDto Address);
