using RealEstate_WebAPI.Services;
using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.DTOs.ResponseDTOs;

namespace RealEstate_WebAPI.Services.Interfaces
{
    public interface IAgentService : IBaseService<Agent, AgentResponseDTO>
    {
        Task<Agent> GetAgentByUserIdAsync(string userId);

        Task<bool> IsUserAgentAsync(string userId);

        Task<AgentResponseDTO> GetAgentByPropertyIdAsync(int propertyId);

        Task<int> AddAgentAsync(Agent agent);

        Task UpdateAgentAsync(Agent agent);
    }
}
