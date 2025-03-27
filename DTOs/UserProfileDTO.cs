using Microsoft.AspNetCore.Http; // For IFormFile
using System.ComponentModel.DataAnnotations;

namespace RealEstate_WebAPI.Models
{
    public class UserProfileDTO
    {
        public string Id { get; set; } // Will be populated from the JWT token

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string? PhoneNumber { get; set; }

        public IFormFile? ProfileImageFile { get; set; } // For file upload

        public string? ProfilePicture { get; set; } // Alternative URL if no file uploaded

        public bool IsAgent { get; set; } // Indicates if the user is an agent

        // Agent-specific fields (optional, only used if IsAgent is true)
        public string? LicenseNumber { get; set; }
        public string? Agency { get; set; }
        public string? Biography { get; set; }
        public int? YearsOfExperience { get; set; }
    }
}