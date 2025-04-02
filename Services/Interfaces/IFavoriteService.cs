using RealEstate_WebAPI.DTOs;
using RealEstate_WebAPI.DTOs.Others;
using RealEstate_WebAPI.DTOs.ResponseDTOs;
using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.ResponseDTOs;

namespace RealEstate_WebAPI.Services
{
    public interface IFavoriteService : IBaseService<Favorite, PropertyResponseDto>
    {
        Task<IEnumerable<PropertyFavoriteDto>> GetFavoritesByUserIdAsync(string userId);
        Task<bool> IsFavoriteAsync(int propertyId, string userId);
        Task AddFavoriteAsync(int propertyId, string userId);
        Task RemoveFavoriteAsync(int propertyId, string userId);
    }
}