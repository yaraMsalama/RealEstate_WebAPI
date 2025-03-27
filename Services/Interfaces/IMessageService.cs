using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.Services;

namespace RealEstate_WebAPI.Services.Interfaces
{
    public interface IMessageService : IBaseService<Message, Message>
    {
        Task<Message> GetMessageByIdAsync(int id);
        Task<IEnumerable<Message>> GetMessagesByAgentIdAsync(string agentId);
        Task SendContactMessageAsync(ContactAgentViewModel model);
        Task MarkAsReadAsync(int id);
    }
}
