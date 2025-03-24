using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.Services;
using RealEstate_WebAPI.ViewModels.Common;

namespace RealEstate_WebAPI.Services.Interfaces
{
    public interface IReviewService : IBaseService<Review, Review>
    {
        Task<Review> CreateAsync(Review review);

        //Task<IEnumerable<Review>> GetAllAsync();

        Task<int> DeleteAsync(int id);

        //Task<IEnumerable<Review>> GetByPropertyIdAsync(int propertyId);

        Task<double> GetAverageRatingAsync(int propertyId);

        Task<IEnumerable<Review>> GetReviewsByPropertyIdAsync(int propertyId, int page = 1, int pageSize = 10);
        Task<int> GetReviewCountForPropertyAsync(int propertyId);
    }
}
