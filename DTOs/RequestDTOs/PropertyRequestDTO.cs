using RealEstate_WebAPI.Models;

namespace RealEstate_WebAPI.DTOs.Request
{
    public class PropertyRequestDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Area { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public PropertyType Type { get; set; }
        public PropertyStatus Status { get; set; }
        public string? AgentId { get; set; }
        public string? FeaturedImage { get; set; }
        public int? YearBuilt { get; set; }
    }
}