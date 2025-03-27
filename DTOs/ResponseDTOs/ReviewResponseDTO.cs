﻿namespace RealEstate_WebAPI.DTOs.ResponseDTOs
{
    public class ReviewResponseDTO
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public int PropertyId { get; set; }
        public string? UserId { get; set; }
    }
}