namespace RealEstate_WebAPI.Models
{
    public class UserInfoDTO
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserType { get; set; }
        public string PhoneNumber { get; set; }
        public string? ProfilePicture { get; internal set; }
        public bool IsAgent { get; internal set; }
        public string LicenseNumber { get; internal set; }
        public string Agency { get; internal set; }
        public string Biography { get; internal set; }
        public int YearsOfExperience { get; internal set; }
    }
}