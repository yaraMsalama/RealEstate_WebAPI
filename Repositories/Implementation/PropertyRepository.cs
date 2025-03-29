using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RealEstate_WebAPI.Data;
using RealEstate_WebAPI.DTOs;
using RealEstate_WebAPI.DTOs.Others;
using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.Repositories.Interfaces;

namespace RealEstate_WebAPI.Repositories.Implementation
{
    public class PropertyRepository : BaseRepository<Property>, IPropertyRepository
    {
        public PropertyRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<Property> GetByIdAsync(int id)
        {
            return await _context.Properties
                .Include(p => p.Agent)
                .Include(p => p.Images)
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public override async Task<IEnumerable<Property>> GetAllAsync()
        {
            return await _context.Properties
                .Include(p => p.Agent)
                .Include(p => p.Images)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public override async Task<int> AddAsync(Property entity)
        {
            _context.Properties.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public override async Task UpdateAsync(Property entity)
        {
            _context.Properties.Update(entity);
            await _context.SaveChangesAsync();
        }

        public override async Task DeleteAsync(Property entity)
        {
            _context.Properties.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Property>> GetByAgentIdAsync(string agentId)
        {
            return await _context.Properties
                .Include(p => p.Agent)
                .Include(p => p.Images)
                .Where(p => p.AgentId == agentId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Property>> SearchAsync(PropertySearchFilterDTO filter)
        {
            var query = _context.Properties
                .Include(p => p.Agent)
                .Include(p => p.Images)
                .AsQueryable();

            // Filter by property type
            if (filter.Type.HasValue)
            {
                query = query.Where(p => p.Type == filter.Type.Value);
            }

            // Price range filtering
            if (filter.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= (decimal)filter.MinPrice.Value);
            }
            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= (decimal)filter.MaxPrice.Value);
            }

            // Location filtering
            if (!string.IsNullOrEmpty(filter.Location))
            {
                query = query.Where(p =>
                    p.City.Contains(filter.Location) ||
                    p.State.Contains(filter.Location) ||
                    p.ZipCode.Contains(filter.Location) ||
                    p.Address.Contains(filter.Location));
            }

            // Bedrooms filtering
            if (filter.MinBedrooms.HasValue)
            {
                query = query.Where(p => p.Bedrooms >= filter.MinBedrooms.Value);
            }
            if (filter.MaxBedrooms.HasValue)
            {
                query = query.Where(p => p.Bedrooms <= filter.MaxBedrooms.Value);
            }

            // Bathrooms filtering
            if (filter.MinBathrooms.HasValue)
            {
                query = query.Where(p => p.Bathrooms >= filter.MinBathrooms.Value);
            }
            if (filter.MaxBathrooms.HasValue)
            {
                query = query.Where(p => p.Bathrooms <= filter.MaxBathrooms.Value);
            }

            // Square footage filtering
            if (filter.MinSquareFeet.HasValue)
            {
                query = query.Where(p => p.Area >= filter.MinSquareFeet.Value);
            }
            if (filter.MaxSquareFeet.HasValue)
            {
                query = query.Where(p => p.Area <= filter.MaxSquareFeet.Value);
            }

            // Property features
            if (filter.HasGarage.HasValue)
            {
                query = query.Where(p => p.HasGarage == filter.HasGarage.Value);
            }
            if (filter.HasPool.HasValue)
            {
                query = query.Where(p => p.HasPool == filter.HasPool.Value);
            }

            // Days on market filtering
            if (filter.MaxDaysOnMarket.HasValue)
            {
                var cutoffDate = DateTime.UtcNow.AddDays(-filter.MaxDaysOnMarket.Value);
                query = query.Where(p => p.CreatedAt >= cutoffDate);
            }

            // Sorting
            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                query = filter.SortBy.ToLower() switch
                {
                    "price" => filter.SortDescending 
                        ? query.OrderByDescending(p => p.Price) 
                        : query.OrderBy(p => p.Price),
                    "bedrooms" => filter.SortDescending 
                        ? query.OrderByDescending(p => p.Bedrooms) 
                        : query.OrderBy(p => p.Bedrooms),
                    "bathrooms" => filter.SortDescending 
                        ? query.OrderByDescending(p => p.Bathrooms) 
                        : query.OrderBy(p => p.Bathrooms),
                    "squarefeet" => filter.SortDescending 
                        ? query.OrderByDescending(p => p.Area) 
                        : query.OrderBy(p => p.Area),
                    "date" => filter.SortDescending 
                        ? query.OrderByDescending(p => p.CreatedAt) 
                        : query.OrderBy(p => p.CreatedAt),
                    _ => query.OrderByDescending(p => p.CreatedAt)
                };
            }
            else
            {
                query = query.OrderByDescending(p => p.CreatedAt);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetFeaturedPropertiesAsync(int count)
        {
            return await _context.Properties
                .Include(p => p.Agent)
                .Include(p => p.Images)
                .Where(p => p.IsFeatured)
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetRecentPropertiesAsync(int count)
        {
            return await _context.Properties
                .Include(p => p.Agent)
                .Include(p => p.Images)
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<int> GetPropertyCountAsync()
        {
            return await _context.Properties.CountAsync();
        }
    }
}