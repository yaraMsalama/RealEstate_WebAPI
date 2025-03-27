namespace RealEstate_WebAPI.DTOs
{
    public class PropertyResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public double SquareFeet { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string AgentId { get; set; }
        public string AgentName { get; set; }
        public string AgentPhone { get; set; }
        public string AgentEmail { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FeaturedImageUrl { get; set; }
        public List<string> ImageUrls { get; set; }
        public IFormFile ImageUpload { get; set; }
        public IEnumerable<IFormFile> AdditionalImages { get; set; }
        public bool IsFavorite { get; set; }
        public int YearBuilt { get; set; }
    }

    public class PropertySearchResponseDTO
    {
        public List<PropertyResponseDTO> Properties { get; set; }
        public PropertySearchFilterDTO Filters { get; set; }
        public PaginationDTO Pagination { get; set; }
    }

    public class PaginationDTO
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int PageSize { get; set; }
    }
}