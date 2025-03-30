using AutoMapper;
using RealEstate_WebAPI.DTOs;
using RealEstate_WebAPI.Models;

namespace RealEstate_WebAPI.Repositories
{
    public interface IPropertyRepository : IBaseRepository<Property>
    {
        Task<int> AddAsync(Property entity);
        Task<IEnumerable<Property>> GetByAgentIdAsync(string agentId);
        Task<Action<IMappingOperationOptions<object, void>>> GetByIdAsync(object id);
        Task<IEnumerable<Property>> SearchAsync(PropertySearchFilterDTO filter);
        //Task SearchAsync(PropertySearchFilterDTO filter);
    }
}