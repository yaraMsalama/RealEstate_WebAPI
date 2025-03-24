using Microsoft.EntityFrameworkCore;
using RealEstate_WebAPI.Data;
using RealEstate_WebAPI.Infrastructure.Repositories;
using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.Repositories;

namespace RealEstate_WebAPI.Repositories.Implementation
{
    public class FavoriteRepository : BaseRepository<Favorite>, IFavoriteRepository
    {
        public FavoriteRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task DeleteAsync(int propertyId, string userId)
        {
            var favorite = await _context.Favorites
           .Include(f => f.Property)
           .Include(f => f.User)
           .FirstOrDefaultAsync(f => f.PropertyId == propertyId && f.UserId == userId);

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<Favorite> GetByUserIdAsync(string userId)
        {
            return await _context.Favorites.FirstOrDefaultAsync(f => f.UserId == userId);
        }

        public async Task<IEnumerable<Favorite>> GetAllByUserIdAsync(string userId)
        {
            return await _context.Favorites.Include(f => f.Property).ToListAsync();
        }

        public async Task<bool> IsFavoriteAsync(int propertyId, string userId)
        {
            return await _context.Favorites
                .AnyAsync(f => f.PropertyId == propertyId && f.UserId == userId);
        }

    }
}