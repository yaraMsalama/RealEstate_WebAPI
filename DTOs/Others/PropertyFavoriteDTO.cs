namespace RealEstate_WebAPI.DTOs.Others
{
    public class PropertyFavoriteDTO
    {
      
        public class PropertyFavoriteDto
        {
            public int Id { get; set; }

            // Favorite-specific properties
            public int FavoriteId { get; set; }
            public DateTime DateSaved { get; set; }
            public string UserId { get; set; }

            // Core property information
            public int PropertyId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int SquareFeet { get; set; }
            public int Bedrooms { get; set; }
            public int Bathrooms { get; set; }

            // Location information
            public string Address { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZipCode { get; set; }
            public string Street { get; set; }

            // Property status and type information
            public string Type { get; set; }  // Changed from PropertyType to string for better serialization
            public string Status { get; set; }  // Changed from PropertyStatus to string for better serialization

            // Image data
            public string CoverImageUrl { get; set; }  // Renamed for clarity
            public List<string> AdditionalImageUrls { get; set; } = new List<string>();  // Initialize to empty list

            // Agent information
            public string AgentName { get; set; }
            public string AgentId { get; set; }
            public string AgentPhone { get; set; }
            public string AgentEmail { get; set; }
        }
    }
}
