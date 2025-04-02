using RealEstate_WebAPI.Models;

namespace RealEstate_WebAPI.DTOs.RequestDTOs
{
    public class UserRequestDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public UserType? UserType { get; set; }
        public string? UserImageURL { get; set; }
    }
}