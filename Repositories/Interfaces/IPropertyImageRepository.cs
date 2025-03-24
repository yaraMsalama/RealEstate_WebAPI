using RealEstate_WebAPI.Models;

namespace RealEstate_WebAPI.Repositories
{
    public interface IPropertyImageRepository : IBaseRepository<PropertyImage>
    {
        Task<PropertyImage> GetByUrlAsync(int propertyId, string imageUrl);
        Task DeletePropertyImagesAsync(int propertyId);
    }
}