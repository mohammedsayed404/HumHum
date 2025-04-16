using System.ComponentModel.DataAnnotations;

namespace Shared.ViewModels
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        [Display(Name = "Current Role")]
        public string CurrentRole { get; set; }

        [Display(Name = "New Role")]
        public string SelectedRole { get; set; }

        public List<string> AllRoles { get; set; } = new();
    }

}
