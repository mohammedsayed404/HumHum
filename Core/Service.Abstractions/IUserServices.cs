using Shared;
using Shared.ViewModels;

namespace Service.Abstractions;

public interface IUserServices
{
    //all thing belongs to user 

    string? Id { get; }
    string? UserEmail { get; }

    //Task<ApplicationUserDto> GetCurrentUserByEmail(string userEmail);


    //Task<AddressToReturnDto> GetUserAddress(string userId);


    Task<AddressToReturnDto> GetUserAddressAsync(string userId);


    Task<int> UpdateUserAddressAsync(AddressToUpdateViewModel model);

    Task<ApplicationUserToReturnDto> GetCurrentUserAsync();

    Task<ApplicationUserToReturnDto> GetCurrentUserWithAddressAsync();
    Task<bool> UpdateUserInformationAsync(ApplicationUserToUpdateViewModule model);
}
