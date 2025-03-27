namespace RealEstate_WebAPI.DTOs.Others
{
    public class EditUserRolesDTO
    {
        public string UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}