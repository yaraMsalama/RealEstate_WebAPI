using RealEstate_WebAPI.DTOs.Others;
using RealEstate_WebAPI.Models;

namespace RealEstate_WebAPI.DTOs
{
    public class PropertySearchFilterDTO
    {
        public string Location { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public int? MinBedrooms { get; set; }
        public int? MaxBedrooms { get; set; }
        public int? MinBathrooms { get; set; }
        public int? MaxBathrooms { get; set; }
        public double? MinSquareFeet { get; set; }
        public double? MaxSquareFeet { get; set; }
        public List<string> PropertyTypes { get; set; }
        public bool? HasGarage { get; set; }
        public bool? HasPool { get; set; }
        public int? MaxDaysOnMarket { get; set; }
        public string SortBy { get; set; }
        public bool SortDescending { get; set; }
        public PropertyType? Type { get; set; }
        public PaginationDto Pagination { get; internal set; }
        public PropertySearchFilterDTO Filters { get; internal set; }
        public List<PropertyResponseDto> Properties { get; internal set; }
    }
}