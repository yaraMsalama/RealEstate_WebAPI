using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RealEstate_WebAPI.DTOs;
using RealEstate_WebAPI.DTOs.ResponseDTOs;

namespace RealEstate_WebAPI.Services
{
    public interface IPropertyService : IBaseService<Property, PropertyResponseDTO>
    {
        Task<PropertyResponseDTO> GetPropertyByIdAsync(int id, string userId);
        Task<IEnumerable<PropertyResponseDTO>> GetAllPropertiesAsync(string userId);
        Task<IEnumerable<PropertyResponseDTO>> GetPropertiesByAgentIdAsync(string agentId, string userId);
        Task<PropertySearchResponseDTO> SearchPropertiesAsync(PropertySearchFilterDTO filter, string userId, int page, int pageSize);
        Task<int> AddPropertyAsync(PropertyResponseDTO property, string agentId);
        Task<int> UpdatePropertyAsync(PropertyResponseDTO model, string userId);
        Task DeletePropertyAsync(int id, string agentId);
        Task UpdatePropertyImagesAsync(PropertyImageResponseDTO dto);
        Task DeletePropertyImageAsync(int propertyId, string imageUrl);
        Task<PropertyResponseDTO> GetPropertyViewModelByIdAsync(int id);
    }
}