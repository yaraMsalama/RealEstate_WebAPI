﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.Repositories;
using RealEstate_WebAPI.DTOs.ResponseDTOs; // Use DTOs instead of ViewModels
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate_WebAPI.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IdentityRole>> GetAllRolesAsync()
        {
            return await _roleRepository.GetAllRolesAsync();
        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            // Check if role already exists
            var roleExists = await _roleRepository.RoleExistsAsync(roleName);
            if (roleExists)
                return false;

            // Create new role
            var role = new IdentityRole(roleName);
            var result = await _roleRepository.CreateRoleAsync(role);
            return result.Succeeded;
        }

        public async Task<EditRoleDTO> GetRoleForEditAsync(string id)
        {
            var role = await _roleRepository.GetRoleByIdAsync(id);
            if (role == null)
                return null;

            var model = new EditRoleDTO
            {
                RoleId = role.Id,
                RoleName = role.Name
                // Note: OriginalRoleName and Users are not in EditRoleDTO
            };

            // Get all users in this role
            var allUsers = await _roleRepository.GetAllUsersAsync();
            var usersInRole = new List<string>();
            foreach (var user in allUsers)
            {
                var userRoles = await _roleRepository.GetUserRolesAsync(user);
                if (userRoles.Contains(role.Name))
                {
                    usersInRole.Add(user.UserName);
                }
            }

            // Since EditRoleDTO doesn't have a Users property, you might want to extend it
            // For now, we'll return the basic DTO
            return model;
        }

        public async Task<bool> UpdateRoleAsync(EditRoleDTO model)
        {
            var role = await _roleRepository.GetRoleByIdAsync(model.RoleId);
            if (role == null)
                return false;

            // Check if the new name already exists (if we're changing the name)
            if (role.Name != model.RoleName)
            {
                var roleExists = await _roleRepository.RoleExistsAsync(model.RoleName);
                if (roleExists)
                    return false; // Role with new name already exists
            }

            // Update the role name
            role.Name = model.RoleName;
            role.NormalizedName = model.RoleName.ToUpper();

            var result = await _roleRepository.UpdateRoleAsync(role);
            return result.Succeeded;
        }

        public async Task<bool> DeleteRoleAsync(string id)
        {
            var role = await _roleRepository.GetRoleByIdAsync(id);
            if (role == null)
                return false;

            // Check if any users are assigned to this role
            var usersInRole = await _roleRepository.GetUsersInRoleAsync(role.Name);
            if (usersInRole.Any())
            {
                return false; // Role is in use by users, cannot delete
            }

            var result = await _roleRepository.DeleteRoleAsync(role);
            return result.Succeeded;
        }

        public async Task<IEnumerable<UserRolesDTO>> GetUsersWithRolesAsync()
        {
            var users = await _roleRepository.GetAllUsersAsync();
            var userRolesDTOs = new List<UserRolesDTO>();

            foreach (var user in users)
            {
                var userRoles = await _roleRepository.GetUserRolesAsync(user);
                userRolesDTOs.Add(new UserRolesDTO
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = string.Join(", ", userRoles)
                });
            }

            return userRolesDTOs;
        }

        public async Task<EditUserRolesDTO> GetUserRolesForEditAsync(string userId)
        {
            var user = await _roleRepository.GetUserByIdAsync(userId);
            if (user == null)
                return null;

            var userRoles = await _roleRepository.GetUserRolesAsync(user);
            var allRoles = await _roleRepository.GetAllRolesAsync();

            var model = new EditUserRolesDTO
            {
                UserId = user.Id,
                UserName = user.UserName,
                UserRoles = allRoles.Select(r => new RoleSelection
                {
                    RoleId = r.Id,
                    RoleName = r.Name,
                    IsSelected = userRoles.Contains(r.Name)
                }).ToList()
            };

            return model;
        }

        public async Task<bool> UpdateUserRolesAsync(EditUserRolesDTO model)
        {
            var user = await _roleRepository.GetUserByIdAsync(model.UserId);
            if (user == null)
                return false;

            // Get current roles
            var userRoles = await _roleRepository.GetUserRolesAsync(user);

            // First remove all existing roles
            if (userRoles.Any())
            {
                var removeResult = await _roleRepository.RemoveUserFromRolesAsync(user, userRoles);
                if (!removeResult.Succeeded)
                    return false;
            }

            // Get only the first selected role (to enforce single role)
            var selectedRole = model.UserRoles.FirstOrDefault(r => r.IsSelected)?.RoleName;

            // Add only one role if selected
            if (!string.IsNullOrEmpty(selectedRole))
            {
                var addResult = await _roleRepository.AddUserToRoleAsync(user, selectedRole);
                return addResult.Succeeded;
            }

            return true; // Successfully removed all roles if none selected
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _roleRepository.GetUserByIdAsync(userId);
            if (user == null)
                return false;

            // First get all roles the user is in
            var userRoles = await _roleRepository.GetUserRolesAsync(user);

            // Remove user from all roles before deletion
            if (userRoles.Any())
            {
                var removeResult = await _roleRepository.RemoveUserFromRolesAsync(user, userRoles);
                if (!removeResult.Succeeded)
                    return false;
            }

            // Now delete the user
            var result = await _roleRepository.DeleteUserAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleRepository.RoleExistsAsync(roleName);
        }

        public async Task<IdentityRole> GetRoleByIdAsync(string id)
        {
            return await _roleRepository.GetRoleByIdAsync(id);
        }

        public async Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName)
        {
            return await _roleRepository.GetUsersInRoleAsync(roleName);
        }
    }
}