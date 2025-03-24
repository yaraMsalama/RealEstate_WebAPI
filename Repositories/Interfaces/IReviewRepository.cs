using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.Repositories;

namespace RealEstate_WebAPI.Repositories.Interfaces
{
    public interface IReviewRepository : IBaseRepository<Review>
    {
        Task<IEnumerable<Review>> GetAllByPropertyIdAsync(int propertyId);

        Task<double> GetAverageRatingAsync(int propertyId);

        Task<int> DeleteAsync(int id);

        Task<Review> GetByIdAsync(object id);
    }
}
