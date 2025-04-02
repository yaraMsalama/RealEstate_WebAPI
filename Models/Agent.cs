using RealEstate_WebAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate_WebAPI.Models
{
    public class Agent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public string LicenseNumber { get; set; }

        public string Agency { get; set; }

        public string Biography { get; set; }

        public int YearsOfExperience { get; set; }

        public string? ProfileImageUrl { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public virtual List<Property>? Properties { get; set; } = new List<Property>();
    }
}
