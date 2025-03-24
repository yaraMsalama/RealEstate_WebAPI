using System.ComponentModel.DataAnnotations;

namespace RealEstate_WebAPI.ViewModels.Roles
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "Role name is required")]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}
