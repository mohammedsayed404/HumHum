using System.Security.Claims;

namespace HumHum.Mock;

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
