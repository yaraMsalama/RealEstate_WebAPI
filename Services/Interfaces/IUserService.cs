using RealEstate_WebAPI.DTOs.Others;
using RealEstate_WebAPI.Models;


namespace RealEstate_WebAPI.Services
{
    public interface IUserService : IBaseService<ApplicationUser, UserProfileDTO>
    {
        Task<ApplicationUser> GetUserByIdAsync(string id);
        Task UpdateUserAsync(ApplicationUser user);
        Task AssignRoleAsync(string userId, string role);

    }
}