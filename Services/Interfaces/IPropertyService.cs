using RealEstate_WebAPI.DTOs;
using RealEstate_WebAPI.DTOs.ResponseDTOs;
using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.ResponseDTOs;
using PropertyRequestDto = RealEstate_WebAPI.DTOs.PropertyRequestDto;

namespace RealEstate_WebAPI.Services
{
    public interface IPropertyService : IBaseService<Property, PropertyResponseDto>
    {
        Task<PropertyResponseDto> GetPropertyByIdAsync(int id, string userId);
        Task<IEnumerable<PropertyResponseDto>> GetAllPropertiesAsync(string userId);
        Task<IEnumerable<PropertyResponseDto>> GetPropertiesByAgentIdAsync(string agentId, string userId);
        Task<PropertySearchResponseDto> SearchPropertiesAsync(PropertySearchFilterDTO filter, string userId, int page, int pageSize);
        Task<int> AddPropertyAsync(PropertyRequestDto property, string userId);
        Task<int> UpdatePropertyAsync(PropertyRequestDto model, string userId);
        Task DeletePropertyAsync(int id, string agentId);
        //Task UpdatePropertyImagesAsync(PropertyImageResponseDTO dto);
        Task DeletePropertyImageAsync(int propertyId, string imageUrl);
    }
}