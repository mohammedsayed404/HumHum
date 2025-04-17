using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity;

public class ApplicationUser : IdentityUser
{
    public string DisplayName { get; set; } = string.Empty;

    public Address Address { get; set; } = null!;

    public bool HasSeenTour { get; set; } = false;

}



