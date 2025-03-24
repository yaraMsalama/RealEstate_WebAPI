using Microsoft.EntityFrameworkCore;
using RealEstate_WebAPI.Repositories;
using RealEstate_WebAPI.Data;
using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

namespace RealEstate_WebAPI.Repositories
{
    public class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepository(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public Task<ApplicationUser> GetByIdAsync(string id)
        {
            return _context.Users
                .Include(u => u.Properties)
                .Include(u => u.Favorites)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<ApplicationUser>> GetByRoleAsync(string role)
        {
            var identityRole = await _roleManager.FindByNameAsync(role);
            if (identityRole == null)
                return new List<ApplicationUser>();

            var usersInRole = await _context.UserRoles
                .Where(ur => ur.RoleId == identityRole.Id)
                .Select(ur => ur.UserId)
                .ToListAsync();

            return await _context.Users
                .Include(u => u.Properties)
                .Include(u => u.Favorites)
                .Where(u => usersInRole.Contains(u.Id))
                .ToListAsync();
        }


    }
}