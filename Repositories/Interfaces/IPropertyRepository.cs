using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.ViewModels.Property;

namespace RealEstate_WebAPI.Repositories
{
    public interface IPropertyRepository : IBaseRepository<Property>
    {
        Task<int> AddAsync(Property entity);
        Task<IEnumerable<Property>> GetByAgentIdAsync(string agentId);
        Task<IEnumerable<Property>> SearchAsync(PropertySearchFilterViewModel filter);
    }
}