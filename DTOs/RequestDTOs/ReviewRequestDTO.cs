using System.ComponentModel.DataAnnotations;

namespace RealEstate_WebAPI.DTOs.RequestDTOs
{
    public class ReviewRequestDTO
    {
        [Range(1, 5)]
        public int Rating { get; set; }
        [StringLength(1000)]
        public string Comment { get; set; }
        public int PropertyId { get; set; }
        public string? UserId { get; set; }
    }
}