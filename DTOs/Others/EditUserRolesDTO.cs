namespace RealEstate_WebAPI.DTOs.Others
{
    public class EditUserRolesDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
        public List<RoleSelection> UserRoles { get; set; }
        public string RoleId { get; internal set; }
        public string? RoleName { get; internal set; }
    }
}