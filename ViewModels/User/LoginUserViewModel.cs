using System.ComponentModel.DataAnnotations;

namespace RealEstate_WebAPI.ViewModels.User
{
    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RemeberMe { get; set; }
    }
}
