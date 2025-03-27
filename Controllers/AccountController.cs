using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RealEstate_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAgentService _agentService;
        private readonly IJwtService _jwtService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IAgentService agentService,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _agentService = agentService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO userFromRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userModel = new ApplicationUser
            {
                FirstName = userFromRequest.FirstName,
                LastName = userFromRequest.LastName,
                Email = userFromRequest.Email,
                UserName = userFromRequest.Username,
                UserType = userFromRequest.UserType,
                PhoneNumber = userFromRequest.PhoneNumber ?? "0000000"
            };

            IdentityResult result = await _userManager.CreateAsync(userModel, userFromRequest.Password);

            if (result.Succeeded)
            {
                // Assign role based on UserType
                if (userFromRequest.UserType == UserType.Agent)
                {
                    await _userManager.AddToRoleAsync(userModel, "Agent");

                    // Create the Agent entity
                    var agent = new Agent
                    {
                        UserId = userModel.Id,
                        LicenseNumber = "Pending",
                        Agency = "Pending",
                        Biography = "",
                        YearsOfExperience = 0,
                        User = userModel
                    };

                    await _agentService.AddAgentAsync(agent);
                }
                else
                {
                    await _userManager.AddToRoleAsync(userModel, "User");
                }

                // Get user roles
                var roles = await _userManager.GetRolesAsync(userModel);

                // Generate JWT token
                var token = _jwtService.GenerateToken(userModel.Id, userModel.UserName, roles);

                return Ok(new
                {
                    Token = token,
                    UserId = userModel.Id,
                    Username = userModel.UserName,
                    UserType = userModel.UserType.ToString()
                });
            }

            // Return errors if registration failed
            var errors = new List<string>();
            foreach (var error in result.Errors)
            {
                errors.Add(error.Description);
            }

            return BadRequest(new { Errors = errors });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO userFromRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userFromDatabase = await _userManager.FindByNameAsync(userFromRequest.Username);

            if (userFromDatabase == null)
                return Unauthorized(new { Message = "Invalid username or password" });

            var passwordValid = await _userManager.CheckPasswordAsync(userFromDatabase, userFromRequest.Password);

            if (!passwordValid)
                return Unauthorized(new { Message = "Invalid username or password" });

            // Add custom claims
            var claims = new List<Claim>
            {
                new Claim("UserType", userFromDatabase.UserType.ToString())
            };

            // Get user roles
            var roles = await _userManager.GetRolesAsync(userFromDatabase);

            // Generate JWT token
            var token = _jwtService.GenerateToken(userFromDatabase.Id, userFromDatabase.UserName, roles);

            return Ok(new
            {
                Token = token,
                UserId = userFromDatabase.Id,
                Username = userFromDatabase.UserName,
                UserType = userFromDatabase.UserType.ToString()
            });
        }

        [HttpPost("logout")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Logout()
        {
            // In JWT-based authentication, the client handles logout by removing the token
            return Ok(new { Message = "Logged out successfully" });
        }

        [HttpPost("external-login")]
        public IActionResult ExternalLogin(string provider)
        {
            // External login needs to be reimplemented for API - this is a placeholder
            return BadRequest(new { Message = "External login not implemented for the API" });
        }

        [HttpGet("user-info")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUserInfo()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            var userInfo = new UserInfoDTO
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserType = user.UserType.ToString(),
                PhoneNumber = user.PhoneNumber
            };

            return Ok(userInfo);
        }

        [HttpGet("user-types")]
        public IActionResult GetUserTypes()
        {
            var userTypes = Enum.GetValues(typeof(UserType))
                .Cast<UserType>()
                .Select(e => new
                {
                    Value = (int)e,
                    Name = e.ToString()
                });

            return Ok(userTypes);
        }
    }
}