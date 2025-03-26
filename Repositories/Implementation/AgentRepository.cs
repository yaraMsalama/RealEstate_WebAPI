using Microsoft.EntityFrameworkCore;
using RealEstate_WebAPI.Data;
using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.Repositories.Interfaces;
using RealEstate_WebAPI.DTOs.ResponseDTOs; // Use DTOs instead of ViewModels
using System.Threading.Tasks;
using RealEstate_WebAPI.Infrastructure.Repositories;

namespace RealEstate_WebAPI.Repositories
{
    public class AgentRepository : BaseRepository<Agent>, IAgentRepository
    {
        private readonly ApplicationDbContext _context;

        public AgentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Agent> GetByUserIdAsync(string userId)
        {
            return await _dbSet
                .Include(a => a.User)
                .Include(a => a.Properties)
                .FirstOrDefaultAsync(a => a.UserId == userId);
        }

        public async Task<AgentResponseDTO> GetByPropertyIdAsync(int propertyId)
        {
            var property = await _context.Properties.FindAsync(propertyId);
            if (property == null)
            {
                return null;
            }

            if (property.AgentId != null)
            {
                var agent = await _dbSet
                    .Include(a => a.User)
                    .FirstOrDefaultAsync(a => a.UserId == property.AgentId);
                if (agent != null)
                {
                    return MapAgentToAgentViewModel(agent);
                }
            }

            return null;
        }

        public AgentResponseDTO MapAgentToAgentViewModel(Agent agent)
        {
            return new AgentResponseDTO
            {
                Id = agent.Id,
                FullName = $"{agent.User.FirstName} {agent.User.LastName}", // Fixed concatenation with a space
                Email = agent.User.Email,
                PhoneNumber = agent.User.PhoneNumber,
                PropertyCount = agent.Properties?.Count ?? 0, // Null check for safety
                LicenseNumber = agent.LicenseNumber,
                Agency = agent.Agency,
                Biography = agent.Biography,
                YearsOfExperience = agent.YearsOfExperience,
                ProfileImageUrl = agent.User.UserImageURL
            };
        }
    }
}