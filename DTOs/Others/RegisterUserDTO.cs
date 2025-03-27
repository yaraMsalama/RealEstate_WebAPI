using RealEstate_WebAPI.Models;

namespace RealEstate_WebAPI.DTOs.Others
{
    public class RegisterUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserType? UserType { get; set; }
        public string? PhoneNumber { get; set; }
    }
}