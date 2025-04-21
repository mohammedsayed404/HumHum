using System.ComponentModel.DataAnnotations;

namespace Shared.ViewModels;
public class ExternalLoginConfirmationViewModel
{

    // Add other required properties here, e.g.,
    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
}

