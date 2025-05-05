using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Presentation.Mock;

public class MockCurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MockCurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }


    //test test

    public string? Id => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

}
