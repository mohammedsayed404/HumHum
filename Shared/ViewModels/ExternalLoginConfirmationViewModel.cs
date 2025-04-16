using System.ComponentModel.DataAnnotations;

namespace Shared.ViewModels;
public class ExternalLoginConfirmationViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}

