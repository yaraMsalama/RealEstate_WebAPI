namespace RealEstate_WebAPI.Models
{
    public class EditUserRolesDTO
    {
        public string UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}