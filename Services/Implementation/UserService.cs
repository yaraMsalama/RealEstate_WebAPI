﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.Repositories;
using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.DTOs.Others;


namespace RealEstate_WebAPI.Services
{
    public class UserService : BaseService<ApplicationUser, UserProfileDTO>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(IUserRepository userRepository,
                            IMapper mapper,
                            UserManager<ApplicationUser> userManager,
                            RoleManager<IdentityRole> roleManager) : base(userRepository, mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userRepository = userRepository;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task UpdateUserAsync(ApplicationUser user)
        {
            var agent = new Agent
            {
                ProfileImageUrl = user.UserImageURL,
                UserId = user.Id



            };
            await _userRepository.UpdateAsync(user);
        }

        public async Task AssignRoleAsync(string userId, string role)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            // Check if the role exists, create it if not
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            // Add the user to the role
            await _userManager.AddToRoleAsync(user, role);

            // If you still need to update the user in your repository
            await _userRepository.UpdateAsync(user);
        }
    }
}