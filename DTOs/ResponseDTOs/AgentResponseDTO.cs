namespace RealEstate_WebAPI.DTOs.ResponseDTOs
{
    public class AgentResponseDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } // Added
        public string Email { get; set; } // Added
        public string PhoneNumber { get; set; } // Added
        public int PropertyCount { get; set; } // Added
        public string LicenseNumber { get; set; }
        public string Agency { get; set; }
        public string Biography { get; set; }
        public int YearsOfExperience { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}