﻿using Microsoft.AspNetCore.Identity;
using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.ViewModels.Roles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstate_WebAPI.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<IdentityRole>> GetAllRolesAsync();
        Task<bool> CreateRoleAsync(RoleViewModel model);
        Task<EditRoleViewModel> GetRoleForEditAsync(string id);
        Task<bool> UpdateRoleAsync(EditRoleViewModel model);
        Task<bool> DeleteRoleAsync(string id);


        // User-Role management
        Task<IEnumerable<UserRolesViewModel>> GetUsersWithRolesAsync();
        Task<EditUserRolesViewModel> GetUserRolesForEditAsync(string userId);
        Task<bool> UpdateUserRolesAsync(EditUserRolesViewModel model);

        Task<bool> DeleteUserAsync(string userId);
        Task<bool> RoleExistsAsync(string roleName);

        Task<IdentityRole> GetRoleByIdAsync(string id);
        Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName);

    }
}