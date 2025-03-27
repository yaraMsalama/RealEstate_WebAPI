namespace RealEstate_WebAPI.DTOs.Request
{
    public class AgentRequestDTO
    {
        public string LicenseNumber { get; set; }
        public string Agency { get; set; }
        public string Biography { get; set; }
        public int YearsOfExperience { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}