namespace RealEstate_WebAPI.DTOs.Others
{
    public class EditRoleDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; } 
        public List<RoleSelection> UserRoles { get; set; }
        public string? RoleName { get; internal set; }
    }
}