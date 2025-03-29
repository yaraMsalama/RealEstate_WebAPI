using Microsoft.AspNetCore.Mvc.Rendering;

namespace RealEstate_WebAPI.Services
{
    public interface IBaseService<TEntity, TDto> where TEntity : class where TDto : class
    {
        Task<TDto> GetByIdAsync(object id);
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto> CreateAsync(TDto dto);
        Task UpdateAsync(TDto dto);
        Task DeleteAsync(object id);
        Task<bool> ExistsAsync(object id);
        Task<int> CountAsync();
        List<SelectListItem> GetEnumSelectList<T>();
    }
}