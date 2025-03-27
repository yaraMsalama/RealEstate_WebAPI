using RealEstate_WebAPI.Repositories;
using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.DTOs.ResponseDTOs;

namespace RealEstate_WebAPI.Repositories.Interfaces
{
    public interface IAgentRepository : IBaseRepository<Agent>
    {
        //Task<Agent> GetByIdAsync(int id);
        Task<Agent> GetByUserIdAsync(string userId);
        //Task<IEnumerable<Agent>> GetAllAsync();
        //Task<int> AddAsync(Agent agent);
        //Task UpdateAsync(Agent agent);
        //Task DeleteAsync(int id);

        Task<AgentResponseDTO> GetByPropertyIdAsync(int propertyId);
    }
}
