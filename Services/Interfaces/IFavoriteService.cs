using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.ViewModels.Property;

namespace RealEstate_WebAPI.Services
{
    public interface IFavoriteService : IBaseService<Favorite, PropertyViewModel>
    {
        Task<IEnumerable<PropertyFavoriteViewModel>> GetFavoritesByUserIdAsync(string userId);
        Task<bool> IsFavoriteAsync(int propertyId, string userId);
        Task AddFavoriteAsync(int propertyId, string userId);
        Task RemoveFavoriteAsync(int propertyId, string userId);
    }
}