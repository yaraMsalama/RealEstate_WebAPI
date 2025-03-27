using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using RealEstate_WebAPI.DTOs;
using RealEstate_WebAPI.Repositories.Interfaces;

namespace RealEstate_WebAPI.Services.Implementation
{
    public class PropertyService : BaseService<Property, PropertyResponseDTO>, IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IPropertyImageRepository _propertyImageRepository;
        private readonly IUserService _userService;
        private readonly IReviewRepository _reviewRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PropertyService(
            IPropertyRepository propertyRepository,
            IFavoriteRepository favoriteRepository,
            IPropertyImageRepository propertyImageRepository,
            IWebHostEnvironment webHostEnvironment,
            IUserService userService,
            IReviewRepository reviewRepository,
            IMapper mapper)
            : base(propertyRepository, mapper)
        {
            _propertyRepository = propertyRepository;
            _favoriteRepository = favoriteRepository;
            _propertyImageRepository = propertyImageRepository;
            _webHostEnvironment = webHostEnvironment;
            _userService = userService;
            _reviewRepository = reviewRepository;
        }

        public async Task<int> AddPropertyAsync(PropertyResponseDTO propertyDTO, string agentId)
        {
            var property = new Property
            {
                Title = propertyDTO.Title,
                Description = propertyDTO.Description,
                Price = propertyDTO.Price,
                Bedrooms = propertyDTO.Bedrooms,
                Bathrooms = propertyDTO.Bathrooms,
                Address = propertyDTO.Address,
                City = propertyDTO.City,
                State = propertyDTO.State,
                ZipCode = propertyDTO.ZipCode,
                Latitude = propertyDTO.Latitude,
                Longitude = propertyDTO.Longitude,
                Type = propertyDTO.Type,
                Status = propertyDTO.Status,
                AgentId = agentId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                YearBuilt = propertyDTO.YearBuilt,
                Images = new List<PropertyImage>()
            };

            if (propertyDTO.ImageUpload != null)
            {
                string featuredImagePath = await SaveImageAsync(propertyDTO.ImageUpload, 0);
                property.FeaturedImage = featuredImagePath;
            }

            if (propertyDTO.AdditionalImages != null && propertyDTO.AdditionalImages.Any())
            {
                List<string> imagePaths = new List<string>();
                foreach (var image in propertyDTO.AdditionalImages)
                {
                    string imagePath = await SaveImageAsync(image, 0);
                    imagePaths.Add(imagePath);
                }

                int propertyId = await _propertyRepository.AddAsync(property);

                if (!string.IsNullOrEmpty(property.FeaturedImage))
                {
                    string newPath = await MoveImageToCorrectFolder(property.FeaturedImage, propertyId);
                    property.FeaturedImage = newPath;
                }

                foreach (var imagePath in imagePaths)
                {
                    string newPath = await MoveImageToCorrectFolder(imagePath, propertyId);
                    property.Images.Add(new PropertyImage
                    {
                        PropertyId = propertyId,
                        ImageUrl = newPath
                    });
                }

                await _propertyRepository.UpdateAsync(property);
                return propertyId;
            }
            else
            {
                return await _propertyRepository.AddAsync(property);
            }
        }

        public async Task<IEnumerable<PropertyResponseDTO>> GetAllPropertiesAsync(string userId)
        {
            var properties = await _propertyRepository.GetAllAsync();
            return await MapPropertiesToDTOs(properties, userId);
        }

        public async Task<PropertyResponseDTO> GetPropertyByIdAsync(int id, string userId)
        {
            var property = await _propertyRepository.GetByIdAsync(id);
            if (property == null)
                return null;

            bool isFavorite = !string.IsNullOrEmpty(userId) && await _favoriteRepository.IsFavoriteAsync(id, userId);
            return MapPropertyToDTO(property, isFavorite);
        }

        public async Task<IEnumerable<PropertyResponseDTO>> GetPropertiesByAgentIdAsync(string agentId, string userId)
        {
            var properties = await _propertyRepository.GetByAgentIdAsync(agentId);
            return await MapPropertiesToDTOs(properties, userId);
        }

        public async Task<PropertySearchResponseDTO> SearchPropertiesAsync(PropertySearchFilterDTO filter, string userId, int page, int pageSize)
        {
            var properties = await _propertyRepository.SearchAsync(filter);

            int totalItems = properties.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var paginatedProperties = properties
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var propertyDTOs = await MapPropertiesToDTOs(paginatedProperties, userId);

            return new PropertySearchResponseDTO
            {
                Properties = propertyDTOs.ToList(),
                Filters = filter,
                Pagination = new PaginationDTO
                {
                    CurrentPage = page,
                    TotalPages = totalPages,
                    TotalItems = totalItems,
                    PageSize = pageSize
                }
            };
        }

        public async Task<int> UpdatePropertyAsync(PropertyResponseDTO model, string userId)
        {
            var property = await _propertyRepository.GetByIdAsync(model.Id);
            if (property == null || property.AgentId != userId)
                return 0;

            string oldFeaturedImage = property.FeaturedImage;

            property.Title = model.Title;
            property.Description = model.Description;
            property.Type = model.Type;
            property.Status = model.Status;
            property.Price = model.Price;
            property.Area = model.SquareFeet;
            property.Bedrooms = model.Bedrooms;
            property.Bathrooms = model.Bathrooms;
            property.Address = model.Address;
            property.City = model.City;
            property.State = model.State;
            property.ZipCode = model.ZipCode;
            property.Latitude = model.Latitude;
            property.Longitude = model.Longitude;
            property.UpdatedAt = DateTime.UtcNow;
            property.YearBuilt = model.YearBuilt;

            if (model.ImageUpload != null && model.ImageUpload.Length > 0)
            {
                if (!string.IsNullOrEmpty(oldFeaturedImage))
                    DeleteImageFromServer(oldFeaturedImage);
                property.FeaturedImage = await SaveImageAsync(model.ImageUpload, property.Id);
            }

            if (model.AdditionalImages != null && model.AdditionalImages.Any())
            {
                foreach (var image in model.AdditionalImages)
                {
                    string imagePath = await SaveImageAsync(image, property.Id);
                    await _propertyImageRepository.AddAsync(new PropertyImage
                    {
                        PropertyId = property.Id,
                        ImageUrl = imagePath
                    });
                }
            }

            await _propertyRepository.UpdateAsync(property);
            return property.Id;
        }

        public async Task DeletePropertyAsync(int id, string agentId)
        {
            var property = await _propertyRepository.GetByIdAsync(id);
            if (property == null)
                throw new KeyNotFoundException($"Property with ID {id} not found.");
            if (property.AgentId != agentId)
                throw new UnauthorizedAccessException("You are not authorized to delete this property.");

            if (!string.IsNullOrEmpty(property.FeaturedImage))
                DeleteImageFromServer(property.FeaturedImage);

            if (property.Images != null && property.Images.Any())
            {
                foreach (var image in property.Images)
                    DeleteImageFromServer(image.ImageUrl);
                await _propertyImageRepository.DeletePropertyImagesAsync(id);
            }

            await _propertyRepository.DeleteAsync(property);
        }

        public async Task UpdatePropertyImagesAsync(PropertyImagesDTO viewModel)
        {
            var property = await _propertyRepository.GetByIdAsync(viewModel.PropertyId);
            if (property == null)
                throw new KeyNotFoundException($"Property with ID {viewModel.PropertyId} not found.");

            if (viewModel.FeaturedImage != null)
            {
                if (!string.IsNullOrEmpty(property.FeaturedImage))
                    DeleteImageFromServer(property.FeaturedImage);
                string featuredImagePath = await SaveImageAsync(viewModel.FeaturedImage, property.Id);
                property.FeaturedImage = featuredImagePath;
                await _propertyRepository.UpdateAsync(property);
            }

            if (viewModel.NewImages != null && viewModel.NewImages.Any())
            {
                foreach (var image in viewModel.NewImages)
                {
                    string imagePath = await SaveImageAsync(image, property.Id);
                    await _propertyImageRepository.AddAsync(new PropertyImage
                    {
                        PropertyId = property.Id,
                        ImageUrl = imagePath
                    });
                }
            }
        }

        public async Task DeletePropertyImageAsync(int propertyId, string imageUrl)
        {
            var image = await _propertyImageRepository.GetByUrlAsync(propertyId, imageUrl);
            if (image == null)
                throw new KeyNotFoundException($"Image not found for property ID {propertyId}.");

            DeleteImageFromServer(imageUrl);
            await _propertyImageRepository.DeleteAsync(image);
        }

        public async Task<PropertyResponseDTO> GetPropertyViewModelByIdAsync(int id)
        {
            var property = await _propertyRepository.GetByIdAsync(id);
            if (property == null)
                return null;

            return MapPropertyToDTO(property, false);
        }

        private PropertyResponseDTO MapPropertyToDTO(Property property, bool isFavorite)
        {
            return new PropertyResponseDTO
            {
                Id = property.Id,
                Title = property.Title,
                Description = property.Description,
                Price = property.Price,
                SquareFeet = property.Area,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                Address = property.Address,
                City = property.City,
                State = property.State,
                ZipCode = property.ZipCode,
                Latitude = property.Latitude,
                Longitude = property.Longitude,
                Type = property.Type,
                Status = property.Status,
                CreatedAt = property.CreatedAt,
                AgentId = property.AgentId,
                AgentName = property.Agent?.FirstName,
                FeaturedImageUrl = property.FeaturedImage,
                ImageUrls = property.Images?.Select(i => i.ImageUrl).ToList() ?? new List<string>(),
                IsFavorite = isFavorite,
                YearBuilt = property.YearBuilt
            };
        }

        private async Task<IEnumerable<PropertyResponseDTO>> MapPropertiesToDTOs(IEnumerable<Property> properties, string userId)
        {
            var propertyDTOs = new List<PropertyResponseDTO>();
            foreach (var property in properties)
            {
                bool isFavorite = !string.IsNullOrEmpty(userId) && await _favoriteRepository.IsFavoriteAsync(property.Id, userId);
                propertyDTOs.Add(MapPropertyToDTO(property, isFavorite));
            }
            return propertyDTOs;
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile, int propertyId)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "properties", propertyId.ToString());
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return $"/images/properties/{propertyId}/{uniqueFileName}";
        }

        private void DeleteImageFromServer(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return;

            try
            {
                string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.TrimStart('/'));
                if (File.Exists(fullPath))
                    File.Delete(fullPath);
            }
            catch (Exception)
            {
                // Log error here
            }
        }

        private async Task<string> MoveImageToCorrectFolder(string tempPath, int propertyId)
        {
            string fileName = Path.GetFileName(tempPath);
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "properties", propertyId.ToString());
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string newPath = Path.Combine(uploadsFolder, fileName);
            string fullTempPath = Path.Combine(_webHostEnvironment.WebRootPath, tempPath.TrimStart('/'));

            if (File.Exists(fullTempPath))
            {
                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    using (var sourceStream = new FileStream(fullTempPath, FileMode.Open))
                    {
                        await sourceStream.CopyToAsync(stream);
                    }
                }
                File.Delete(fullTempPath);
            }

            return $"/images/properties/{propertyId}/{fileName}";
        }
    }
}