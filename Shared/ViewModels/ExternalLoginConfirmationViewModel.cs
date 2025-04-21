using Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace Shared.ViewModels;
public class ExternalLoginConfirmationViewModel
{

    [Required]
    [Display(Name = "Full Name")]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public Address Address { get; set; } = null!;
}

