using RealEstate_WebAPI.DTOs;
using RealEstate_WebAPI.DTOs.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstate_WebAPI.Services
{
    public interface IPropertyService : IBaseService<Property, PropertyResponseDto>
    {
        Task<PropertyResponseDto> GetPropertyByIdAsync(int id, string userId);
        Task<IEnumerable<PropertyResponseDto>> GetAllPropertiesAsync(string userId);
        Task<IEnumerable<PropertyResponseDto>> GetPropertiesByAgentIdAsync(string agentId, string userId);
        Task<PropertySearchResponseDto> SearchPropertiesAsync(PropertySearchFilterDto filter, string userId, int page, int pageSize);
        Task<int> AddPropertyAsync(PropertyRequestDto property, string agentId);
        Task<int> UpdatePropertyAsync(PropertyRequestDto model, string userId);
        Task DeletePropertyAsync(int id, string agentId);
        Task UpdatePropertyImagesAsync(PropertyImagesDto dto);
        Task DeletePropertyImageAsync(int propertyId, string imageUrl);
    }
}