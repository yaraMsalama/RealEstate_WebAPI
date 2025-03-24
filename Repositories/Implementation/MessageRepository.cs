using Microsoft.EntityFrameworkCore;
using RealEstate_WebAPI.Data;
using RealEstate_WebAPI.Infrastructure.Repositories;
using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.Repositories.Interfaces;

namespace RealEstate_WebAPI.Repositories.Implementation
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetByAgentIdAsync(string agentId)
        {
            return await _context.Messages
                .Where(m => m.AgentId == agentId)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

    }
}