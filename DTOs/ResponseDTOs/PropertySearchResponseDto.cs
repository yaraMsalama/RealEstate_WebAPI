using RealEstate_WebAPI.DTOs.Others;

namespace RealEstate_WebAPI.DTOs.ResponseDTOs
{
    public class PropertySearchResponseDto
    {
        public List<PropertyResponseDto> Properties { get; set; }
        public PropertySearchFilterDTO Filters { get; set; }
        public PaginationDto Pagination { get; set; }
    }

}
