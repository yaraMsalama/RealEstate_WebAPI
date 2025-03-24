using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.Repositories;

namespace RealEstate_WebAPI.Repositories.Interfaces
{
    public interface IMessageRepository : IBaseRepository<Message>
    {
        Task<IEnumerable<Message>> GetByAgentIdAsync(string agentId);
    }
}
