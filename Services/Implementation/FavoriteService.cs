using AutoMapper;
using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.Repositories;
using RealEstate_WebAPI.Services;
using RealEstate_WebAPI.Repositories.Implementation;
using System.ComponentModel.DataAnnotations.Schema;
using RealEstate_WebAPI.DTOs;
using RealEstate_WebAPI.DTOs.Others;
using static RealEstate_WebAPI.DTOs.Others.PropertyFavoriteDTO;

namespace RealEstate_WebAPI.Services.Implementation
{

    public class FavoriteService : BaseService<Favorite, PropertyResponseDto>, IFavoriteService
    {
        /// <summary>
        /// /mapper
        /// </summary>
        private readonly IFavoriteRepository _repository;
        private readonly IMapper _mapper;
        public FavoriteService(IFavoriteRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task AddFavoriteAsync(int propertyId, string userId)
        {
            // Create a new Favorite object
            var favorite = new Favorite
            {
                PropertyId = propertyId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            // Add the favorite to the repository
            await _repository.AddAsync(favorite);

            // SaveChanges is a separate operation, use 'await' to complete it
            await _repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<PropertyFavoriteDTO>> GetFavoritesByUserIdAsync(string userId)
        {
            var favorites = await _repository.GetAllByUserIdAsync(userId);

            var result = new List<PropertyFavoriteDTO>();
            foreach (var favorite in favorites)
            {
           //     result.Add(FromFavorite(favorite));
            }
            return result;
        }

        public async Task<bool> IsFavoriteAsync(int propertyId, string userId)
        {
            // Input validation
            if (propertyId <= 0)
            {
                throw new ArgumentException("Invalid property ID", nameof(propertyId));
            }

            if (string.IsNullOrEmpty(userId))
            {
                return false; // Cannot be a favorite if user ID is null or empty
            }

            try
            {
                // Make sure _favoriteRepository is not null before using it
                if (_repository == null)
                {
                    throw new InvalidOperationException("Favorite repository is not initialized");
                }

                return await _repository.IsFavoriteAsync(propertyId, userId);
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, $"Error checking favorite status for property {propertyId} and user {userId}");

                // Return false as a fallback instead of propagating the exception
                return false;
            }
        }

        public async Task RemoveFavoriteAsync(int propertyId, string userId)
        {
            await _repository.DeleteAsync(propertyId, userId);
        }


        public static PropertyFavoriteDto FromFavorite(Favorite favorite)
        {
            var property = favorite.Property;
            return new PropertyFavoriteDto
            {
                Id = favorite.Id,
                UserId = favorite.UserId,
                DateSaved = favorite.CreatedAt,
                PropertyId = property.Id,
                Title = property.Title,
                Description = property.Description,
                Price = property.Price,
                Address = property.Address,
                City = property.City.ToString(), // Convert City enum to string
                State = property.State,
                ZipCode = property.ZipCode,
                SquareFeet = property.Area,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                Type = property.Type.ToString(), // Convert PropertyType enum to string
                Status = property.Status.ToString(), // Convert PropertyStatus enum to string
                CoverImageUrl = property.FeaturedImage
            };
        }
    }
    }
