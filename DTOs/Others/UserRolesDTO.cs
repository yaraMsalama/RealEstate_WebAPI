namespace RealEstate_WebAPI.DTOs.Others
{
    public class UserRolesDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Roles { get; set; } // Comma-separated roles
    }
}