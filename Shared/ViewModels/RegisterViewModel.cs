using Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace Shared.ViewModels
{
    public class RegisterViewModel
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public Address Address { get; set; } = null!;
    }
}