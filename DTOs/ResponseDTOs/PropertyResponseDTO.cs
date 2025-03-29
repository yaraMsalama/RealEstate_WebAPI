using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace RealEstate_WebAPI.DTOs
{
    public class PropertyRequestDto
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
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int YearBuilt { get; set; }
        public IFormFile FeaturedImage { get; set; }
        public IEnumerable<IFormFile> AdditionalImages { get; set; }
    }

    public class PropertyResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int SquareFeet { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string AgentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FeaturedImageUrl { get; set; }
        public List<string> ImageUrls { get; set; }
        public bool IsFavorite { get; set; }
        public int YearBuilt { get; set; }
    }

 

    //public class PropertySearchFilterDto
    //{
    //    public decimal? MinPrice { get; set; }
    //    public decimal? MaxPrice { get; set; }
    //    public int? Bedrooms { get; set; }
    //    public int? Bathrooms { get; set; }
    //    public string Type { get; set; }
    //    public string Status { get; set; }
    //    public string City { get; set; }
    //    public string State { get; set; }
    //}

    //public class PropertySearchResponseDto
    //{
    //    public List<PropertyResponseDto> Properties { get; set; }
    //    public PropertySearchFilterDto Filters { get; set; }
    //    public PaginationDto Pagination { get; set; }
    //}

   
}