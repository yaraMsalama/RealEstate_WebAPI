namespace RealEstate_WebAPI.DTOs.ResponseDTOs
{
    public class EditUserRolesDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; } // Existing property for roles as strings
        public List<RoleSelection> UserRoles { get; set; } // Added for role selection
    }
}