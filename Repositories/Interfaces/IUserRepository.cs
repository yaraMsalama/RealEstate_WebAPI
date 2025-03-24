using RealEstate_WebAPI.Models;

namespace RealEstate_WebAPI.Repositories
{
    public interface IUserRepository : IBaseRepository<ApplicationUser>
    {
        Task<ApplicationUser> GetByIdAsync(string id);
        //Task<IEnumerable<ApplicationUser>> GetAllAsync();

        Task<IEnumerable<ApplicationUser>> GetByRoleAsync(string role);
        //Task UpdateAsync(ApplicationUser user);
    }
}