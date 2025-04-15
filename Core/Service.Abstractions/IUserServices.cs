using Shared;

namespace Service.Abstractions;

public interface IUserServices
{
    //all thing belongs to user 


    string? Id { get; }
    string? UserEmail { get; }





    //Task<ApplicationUserDto> GetCurrentUserByEmail(string userEmail);


    Task<AddressToReturnDto> GetUserAddress(string userId);

    Task<AddressToReturnDto> UpdateUserAddress(string userId);


}
