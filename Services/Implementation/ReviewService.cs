using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.Repositories;
using RealEstate_WebAPI.Services;
using RealEstate_WebAPI.Repositories.Interfaces;
using RealEstate_WebAPI.Services.Interfaces;
using RealEstate_WebAPI.ViewModels.Common;
using RealEstate_WebAPI.Repositories.Interfaces;

namespace RealEstate_WebAPI.Services.Implementation
{
    public class ReviewService : BaseService<Review, Review>, IReviewService
    {
        private readonly IReviewRepository _repository;
        private readonly IPropertyRepository _propertyRepository;

        private readonly IMapper mapper;

        public ReviewService(IReviewRepository repository, IPropertyRepository propertyRepository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _propertyRepository = propertyRepository;
        }

        public async Task<Review> CreateAsync(Review review)
        {
            //var property = _propertyRepository.GetByIdAsync(review.PropertyId);
            //var rev = new Review
            //{
            //    PropertyId = review.PropertyId,
            //    Rating = review.Rating,
            //    Comment = review.Comment,
            //    UserId = "0fc97d92-f580-42a3-a9b7-23f378f3caaa",
            //    CreatedAt = DateTime.UtcNow,
            //    Property = property.Result
            //};

            //await _repository.AddAsync(review);
            //await _repository.SaveChangesAsync();
            //return review;
            // Don't assign the Property object, just use the foreign key
            review.CreatedAt = DateTime.UtcNow;

            // Use the userId passed from the controller
            // Don't hardcode the UserId here

            await _repository.AddAsync(review);
            await _repository.SaveChangesAsync();
            return review;
        }

        public async Task<Review> UpdateAsync(int id, ReviewViewModel model, string userId)
        {
            var reviews = await _repository.FindAsync(r => r.Id == id);
            var review = reviews.FirstOrDefault();

            if (review == null)
            {
                throw new KeyNotFoundException($"Review with ID {id} not found");
            }

            if (review.UserId != userId)
            {
                throw new UnauthorizedAccessException("You can only update your own reviews");
            }

            review.Rating = model.Rating;
            review.Comment = model.Comment;

            _repository.UpdateAsync(review);
            await _repository.SaveChangesAsync();
            return review;
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Review>> GetByPropertyIdAsync(int propertyId)
        {
            return await _repository.GetAllByPropertyIdAsync(propertyId);

        }

        public async Task<double> GetAverageRatingAsync(int propertyId)
        {
            return await _repository.GetAverageRatingAsync(propertyId);
        }

        public async Task<Review> GetByIdAsync(object id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity;
        }

        public async Task<IEnumerable<Review>> GetReviewsByPropertyIdAsync(int propertyId, int page = 1, int pageSize = 10)
        {

            int itemsToSkip = (page - 1) * pageSize;

            var reviews = await _repository.FindAsync(r => r.PropertyId == propertyId);

            return reviews
                .OrderByDescending(r => r.CreatedAt)
                .Skip(itemsToSkip)
                .Take(pageSize)
                .ToList();
        }

        public async Task<int> GetReviewCountForPropertyAsync(int propertyId)
        {

            var reviews = await _repository.FindAsync(r => r.PropertyId == propertyId);

            // Return the count
            return reviews.Count();
        }
    }
}
