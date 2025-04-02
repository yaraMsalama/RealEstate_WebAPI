using RealEstate_WebAPI.DTOs.Others;
using RealEstate_WebAPI.ResponseDTOs;

namespace RealEstate_WebAPI.DTOs.ResponseDTOs
{
    public class PropertySearchResponseDto
    {
        public List<PropertyResponseDto> Properties { get; set; }
        public PropertySearchFilterDTO Filters { get; set; }
        public PaginationDto Pagination { get; set; }
    }

}
