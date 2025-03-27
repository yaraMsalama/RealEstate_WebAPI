using Microsoft.AspNetCore.Identity;
using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.DTOs.ResponseDTOs; // Use DTOs instead of ViewModels
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstate_WebAPI.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<IdentityRole>> GetAllRolesAsync();
        Task<bool> CreateRoleAsync(string roleName); // Inferred type as string
        Task<EditRoleDTO> GetRoleForEditAsync(string id);
        Task<bool> UpdateRoleAsync(EditRoleDTO model);
        Task<bool> DeleteRoleAsync(string id);

        // User-Role management
        Task<IEnumerable<EditUserRolesDTO>> GetUsersWithRolesAsync();
        Task<EditUserRolesDTO> GetUserRolesForEditAsync(string userId);
        Task<bool> UpdateUserRolesAsync(EditUserRolesDTO model);

        Task<bool> DeleteUserAsync(string userId);
        Task<bool> RoleExistsAsync(string roleName);

        Task<IdentityRole> GetRoleByIdAsync(string id);
        Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName);
    }
}