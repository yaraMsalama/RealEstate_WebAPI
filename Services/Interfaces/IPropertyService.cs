using RealEstate_WebAPI.DTOs;
using RealEstate_WebAPI.DTOs.Request;
using RealEstate_WebAPI.DTOs.ResponseDTOs;
using RealEstate_WebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstate_WebAPI.Services
{
    public interface IPropertyService : IBaseService<Property, PropertyResponseDto>
    {
        Task<PropertyResponseDto> GetPropertyByIdAsync(int id, string userId);
        Task<IEnumerable<PropertyResponseDto>> GetAllPropertiesAsync(string userId);
        Task<IEnumerable<PropertyResponseDto>> GetPropertiesByAgentIdAsync(string agentId, string userId);
        Task<PropertySearchFilterDTO> SearchPropertiesAsync(PropertySearchFilterDTO filter, string userId, int page, int pageSize);
        Task<int> AddPropertyAsync(PropertyRequestDto property, string agentId);
        Task<int> UpdatePropertyAsync(PropertyRequestDto model, string userId);
        Task DeletePropertyAsync(int id, string agentId);
        Task UpdatePropertyImagesAsync(PropertyImageResponseDTO dto);
        Task DeletePropertyImageAsync(int propertyId, string imageUrl);
    }
}