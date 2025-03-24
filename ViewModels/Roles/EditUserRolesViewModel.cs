namespace RealEstate_WebAPI.ViewModels.Roles
{
    public class EditUserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<RoleSelection> UserRoles { get; set; }
    }
}
