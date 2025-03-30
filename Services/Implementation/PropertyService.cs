using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using RealEstate_WebAPI.DTOs;
using RealEstate_WebAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using RealEstate_WebAPI.DTOs.Request;
using RealEstate_WebAPI.DTOs.ResponseDTOs;
using RealEstate_WebAPI.DTOs.Others;

namespace RealEstate_WebAPI.Services.Implementation
{
    public class PropertyService : BaseService<Property, PropertyResponseDto>, IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IPropertyImageRepository _propertyImageRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PropertyService(
            IPropertyRepository propertyRepository,
            IFavoriteRepository favoriteRepository,
            IPropertyImageRepository propertyImageRepository,
            IWebHostEnvironment webHostEnvironment,
            IMapper mapper)
            : base(propertyRepository, mapper)
        {
            _propertyRepository = propertyRepository;
            _favoriteRepository = favoriteRepository;
            _propertyImageRepository = propertyImageRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<int> AddPropertyAsync(DTOs.PropertyRequestDto propertyDto, string agentId)
        {
            var property = _mapper.Map<Property>(propertyDto);
            property.AgentId = agentId;
            property.CreatedAt = DateTime.UtcNow;
            property.UpdatedAt = DateTime.UtcNow;
            property.Images = new List<PropertyImage>();

            if (propertyDto.FeaturedImage != null)
            {
                property.FeaturedImage = await SaveImageAsync(propertyDto.FeaturedImage, 0);
            }

            int propertyId = await _propertyRepository.AddAsync(property);

            if (!string.IsNullOrEmpty(property.FeaturedImage))
            {
                string newPath = await MoveImageToCorrectFolder(property.FeaturedImage, propertyId);
                property.FeaturedImage = newPath;
            }

            if (propertyDto.AdditionalImages != null && propertyDto.AdditionalImages.Any())
            {
                foreach (var image in propertyDto.AdditionalImages)
                {
                    string imagePath = await SaveImageAsync(image, propertyId);
                    property.Images.Add(new PropertyImage
                    {
                        PropertyId = propertyId,
                        ImageUrl = imagePath
                    });
                }
            }

            await _propertyRepository.UpdateAsync(property);
            return propertyId;
        }

        public async Task<IEnumerable<PropertyResponseDto>> GetAllPropertiesAsync(string userId)
        {
            var properties = await _propertyRepository.GetAllAsync();
            return await MapPropertiesToDtos(properties, userId);
        }

        public async Task<PropertyResponseDto> GetPropertyByIdAsync(int id, string userId)
        {
            var property = await _propertyRepository.GetByIdAsync(id);
            if (property == null) return null;

            bool isFavorite = !string.IsNullOrEmpty(userId) && await _favoriteRepository.IsFavoriteAsync(id, userId);
            return MapPropertyToDto(property, isFavorite);
        }

        public async Task<IEnumerable<PropertyResponseDto>> GetPropertiesByAgentIdAsync(string agentId, string userId)
        {
            var properties = await _propertyRepository.GetByAgentIdAsync(agentId);
            return await MapPropertiesToDtos(properties, userId);
        }

        public async Task<PropertySearchResponseDto> SearchPropertiesAsync(PropertySearchFilterDTO filter, string userId, int page, int pageSize)
        {
            var properties = await _propertyRepository.SearchAsync(filter);
            int totalItems = properties.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var paginatedProperties = properties
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var propertyDtos = await MapPropertiesToDtos(paginatedProperties, userId);

            return new PropertySearchResponseDto
            {
                Properties = propertyDtos.ToList(),
                Filters = filter,
                Pagination = new PaginationDto
                {
                    CurrentPage = page,
                    TotalPages = totalPages,
                    TotalItems = totalItems,
                    PageSize = pageSize
                }
            };
        }

        public async Task<int> UpdatePropertyAsync(DTOs.PropertyRequestDto model, string userId)
        {
            var property = await _propertyRepository.GetByIdAsync(model.Id);
            if (property == null || property.AgentId != userId) return 0;

            _mapper.Map(model, property);
            property.UpdatedAt = DateTime.UtcNow;

            if (model.FeaturedImage != null)
            {
                if (!string.IsNullOrEmpty(property.FeaturedImage))
                    DeleteImageFromServer(property.FeaturedImage);
                property.FeaturedImage = await SaveImageAsync(model.FeaturedImage, property.Id);
            }

            if (model.AdditionalImages != null)
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
            if (property == null) throw new KeyNotFoundException($"Property with ID {id} not found.");
            if (property.AgentId != agentId) throw new UnauthorizedAccessException("Unauthorized to delete this property.");

            if (!string.IsNullOrEmpty(property.FeaturedImage))
                DeleteImageFromServer(property.FeaturedImage);

            if (property.Images != null)
            {
                foreach (var image in property.Images)
                    DeleteImageFromServer(image.ImageUrl);
                await _propertyImageRepository.DeletePropertyImagesAsync(id);
            }

            await _propertyRepository.DeleteAsync(property);
        }

        public async Task UpdatePropertyImagesAsync(PropertyImagesDto dto)
        {
            var property = await _propertyRepository.GetByIdAsync(dto.PropertyId);
            if (property == null) throw new KeyNotFoundException($"Property with ID {dto.PropertyId} not found.");

            if (dto.FeaturedImage != null)
            {
                if (!string.IsNullOrEmpty(property.FeaturedImage))
                    DeleteImageFromServer(property.FeaturedImage);
                property.FeaturedImage = await SaveImageAsync(dto.FeaturedImage, property.Id);
                await _propertyRepository.UpdateAsync(property);
            }

            if (dto.NewImages != null)
            {
                foreach (var image in dto.NewImages)
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
            if (image == null) throw new KeyNotFoundException($"Image not found for property ID {propertyId}.");

            DeleteImageFromServer(imageUrl);
            await _propertyImageRepository.DeleteAsync(image);
        }

        private PropertyResponseDto MapPropertyToDto(Property property, bool isFavorite)
        {
            return new PropertyResponseDto
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
                Type = property.Type.ToString(),
                Status = property.Status.ToString(),
                CreatedAt = property.CreatedAt,
                AgentId = property.AgentId,
                FeaturedImageUrl = property.FeaturedImage,
                ImageUrls = property.Images?.Select(i => i.ImageUrl).ToList() ?? new List<string>(),
                IsFavorite = isFavorite,
                //YearBuilt = property.YearBuilt
            };
        }

        private async Task<IEnumerable<PropertyResponseDto>> MapPropertiesToDtos(IEnumerable<Property> properties, string userId)
        {
            var dtos = new List<PropertyResponseDto>();
            foreach (var property in properties)
            {
                bool isFavorite = !string.IsNullOrEmpty(userId) && await _favoriteRepository.IsFavoriteAsync(property.Id, userId);
                dtos.Add(MapPropertyToDto(property, isFavorite));
            }
            return dtos;
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile, int propertyId)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "properties", propertyId.ToString());
            Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return $"/images/properties/{propertyId}/{uniqueFileName}";
        }

        private void DeleteImageFromServer(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.TrimStart('/'));
            if (File.Exists(fullPath)) File.Delete(fullPath);
        }

        private async Task<string> MoveImageToCorrectFolder(string tempPath, int propertyId)
        {
            string fileName = Path.GetFileName(tempPath);
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "properties", propertyId.ToString());
            Directory.CreateDirectory(uploadsFolder);

            string newPath = Path.Combine(uploadsFolder, fileName);
            string fullTempPath = Path.Combine(_webHostEnvironment.WebRootPath, tempPath.TrimStart('/'));

            if (File.Exists(fullTempPath))
            {
                using (var stream = new FileStream(newPath, FileMode.Create))
                using (var sourceStream = new FileStream(fullTempPath, FileMode.Open))
                {
                    await sourceStream.CopyToAsync(stream);
                }
                File.Delete(fullTempPath);
            }

            return $"/images/properties/{propertyId}/{fileName}";
        }

        //Task<PropertySearchFilterDTO> IPropertyService.SearchPropertiesAsync(PropertySearchFilterDTO filter, string userId, int page, int pageSize)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task UpdatePropertyImagesAsync(PropertyImageResponseDTO dto)
        //{
        //    throw new NotImplementedException();
        //}
    }
}