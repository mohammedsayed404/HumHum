using Microsoft.AspNetCore.Http;
using Service.Abstractions;
using Shared;
using System.Security.Claims;

namespace Services;

internal sealed class UserServices : IUserServices
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserServices(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? Id => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? UserEmail => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
    //public string? cartid => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.d);

    public Task<AddressToReturnDto> GetUserAddress(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<AddressToReturnDto> UpdateUserAddress(string userId)
    {
        throw new NotImplementedException();
    }






}
