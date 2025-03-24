using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RealEstate_WebAPI.Data;
using RealEstate_WebAPI.Infrastructure.Repositories;
using RealEstate_WebAPI.Repositories;
using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.Repositories.Interfaces;
using RealEstate_WebAPI.ViewModels.Agent;

namespace RealEstate_WebAPI.Repositories
{
    public class AgentRepository : BaseRepository<Agent>, IAgentRepository
    {
        public AgentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Agent> GetByUserIdAsync(string userId)
        {
            return await _dbSet
                .Include(a => a.User)
                .Include(a => a.Properties)
                .FirstOrDefaultAsync(a => a.UserId == userId);
        }

        public async Task<AgentViewModel> GetByPropertyIdAsync(int propertyId)
        {
            var property = await _context.Properties.FindAsync(propertyId);
            if (property == null)
            {
                return null;
            }

            if (property.AgentId != null)
            {
                var agent = await _dbSet.Include(a => a.User)
                                       .FirstOrDefaultAsync(a => a.UserId == property.AgentId);
                if (agent != null)
                {
                    return MapAgentToAgentViewModel(agent);
                }
            }

            return null;
        }

        public AgentViewModel MapAgentToAgentViewModel(Agent agent)
        {
            return new AgentViewModel
            {
                FullName = agent.User.FirstName + "" + agent.User.LastName,
                Email = agent.User.Email,
                PhoneNumber = agent.User.PhoneNumber,
                PropertyCount = agent.Properties.Count,
                LicenseNumber = agent.LicenseNumber,
                Agency = agent.Agency,
                Biography = agent.Biography,
                YearsOfExperience = agent.YearsOfExperience,
                ProfileImageUrl = agent.User.UserImageURL
            };
        }


    }
}