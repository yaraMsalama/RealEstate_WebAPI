using RealEstate_WebAPI.Services;
using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.ViewModels.Agent;

namespace RealEstate_WebAPI.Services.Interfaces
{
    public interface IAgentService : IBaseService<Agent, AgentViewModel>
    {
        Task<Agent> GetAgentByUserIdAsync(string userId);

        Task<bool> IsUserAgentAsync(string userId);

        Task<AgentViewModel> GetAgentByPropertyIdAsync(int propertyId);

        Task<int> AddAgentAsync(Agent agent);

        Task UpdateAgentAsync(Agent agent);
    }
}
