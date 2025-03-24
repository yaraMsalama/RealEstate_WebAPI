using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.ViewModels.Admin;
using RealEstate_WebAPI.ViewModels.Agent;
using RealEstate_WebAPI.ViewModels.User;

namespace RealEstate_WebAPI.Services
{
    public interface IUserService : IBaseService<ApplicationUser, UserProfileViewModel>
    {
        Task<ApplicationUser> GetUserByIdAsync(string id);
        Task UpdateUserAsync(ApplicationUser user);
        Task AssignRoleAsync(string userId, string role);

    }
}