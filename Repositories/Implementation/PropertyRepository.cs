using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RealEstate_WebAPI.Data;
using RealEstate_WebAPI.DTOs;
using RealEstate_WebAPI.DTOs.Others;
using RealEstate_WebAPI.Infrastructure.Repositories;
using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.Repositories;

namespace RealEstate_WebAPI.Repositories.Implementation
{
    public class PropertyRepository : BaseRepository<Property>, IPropertyRepository
    {
        public PropertyRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<int> AddAsync(Property entity)
        {
            _context.Properties.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
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

            if (filter.Filters.Type.HasValue)
            {
                query = query.Where(p => p.Type == filter.Filters.Type.Value);
                if (filter.Filters.MinPrice.HasValue)
                {
                    query = query.Where(p => p.Price >= (decimal)filter.Filters.MinPrice.Value);
                }
                if (filter.Filters.MaxPrice.HasValue)
                {
                    query = query.Where(p => p.Price <= (decimal)filter.Filters.MaxPrice.Value);
                }
            }

            if (!string.IsNullOrEmpty(filter.Filters.Location))
            {
                query = query.Where(p =>
                    p.City.Contains(filter.Filters.Location) ||
                    p.State.Contains(filter.Filters.Location) ||
                    p.ZipCode.Contains(filter.Filters.Location) ||
                    p.Address.Contains(filter.Filters.Location));
            }

            if (filter.Filters.MinBedrooms.HasValue)
            {
                query = query.Where(p => p.Bedrooms >= filter.Filters.MinBedrooms.Value);
            }
            if (filter.Filters.MaxBedrooms.HasValue)
            {
                query = query.Where(p => p.Bedrooms <= filter.Filters.MaxBedrooms.Value);
            }
            if (filter.Filters.MinBathrooms.HasValue)
            {
                query = query.Where(p => p.Bathrooms >= filter.Filters.MinBathrooms.Value);
            }

            return await query
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public new async Task<Property> GetByIdAsync(object id)
        {
            return await _context.Properties
                .Include(p => p.Agent)
                .Include(p => p.Images)
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.Id == (int)id);
        }

        public new async Task<IEnumerable<Property>> GetAllAsync()
        {
            return await _context.Properties
                .Include(p => p.Agent)
                .Include(p => p.Images)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }



       
    }
}
