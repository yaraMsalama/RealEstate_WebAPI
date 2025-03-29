using RealEstate_WebAPI.DTOs.Others;
using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.Services;

namespace RealEstate_WebAPI.Services.Interfaces
{
    public interface IMessageService : IBaseService<Message, Message>
    {
        Task<Message> GetMessageByIdAsync(int id);
        Task<IEnumerable<Message>> GetMessagesByAgentIdAsync(string agentId);
        Task SendContactMessageAsync(ContactAgentDTO contant);
        Task MarkAsReadAsync(int id);
    }
}
