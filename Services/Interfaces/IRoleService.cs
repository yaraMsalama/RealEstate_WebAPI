using Microsoft.AspNetCore.Identity;
using RealEstate_WebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using RealEstate_WebAPI.DTOs.Others;

namespace RealEstate_WebAPI.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<IdentityRole>> GetAllRolesAsync();
        Task<bool> CreateRoleAsync(string roleName); // Inferred type as string
        Task<EditUserRolesDTO> GetRoleForEditAsync(string id);
        Task<bool> UpdateRoleAsync(EditUserRolesDTO model);
        Task<bool> DeleteRoleAsync(string id);

        // User-Role management
        Task<IEnumerable<EditUserRolesDTO>> GetUsersWithRolesAsync();
        Task<EditUserRolesDTO> GetUserRolesForEditAsync(string userId);
        Task<bool> UpdateUserRolesAsync(EditUserRolesDTO model);

        Task<bool> DeleteUserAsync(string userId);
        Task<bool> RoleExistsAsync(string roleName);

        Task<IdentityRole> GetRoleByIdAsync(string id);
        Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName);
        Task<bool> CreateRoleAsync(UserRolesDTO roleFromRequest);
    }
}