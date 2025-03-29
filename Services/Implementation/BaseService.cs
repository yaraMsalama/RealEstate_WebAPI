using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealEstate_WebAPI.Repositories;


namespace RealEstate_WebAPI.Services
{
    public class BaseService<TEntity, TDto> : IBaseService<TEntity, TDto>
          where TEntity : class
          where TDto : class
    {
        protected readonly IBaseRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public BaseService(IBaseRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TDto> GetByIdAsync(object id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<TDto>(entity);
        }

        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return MapToDtos(entities);
        }

        public async Task<TDto> CreateAsync(TDto dto)
        {
            var entity = MapToEntity(dto);
            await _repository.AddAsync(entity);
            return _mapper.Map<TDto>(entity);
        }

        public async Task UpdateAsync(TDto dto)
        {
            var entity = MapToEntity(dto);
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(object id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(object id)
        {
            return await _repository.ExistsAsync(e => EF.Property<object>(e, "Id").Equals(id));
        }

        public async Task<int> CountAsync()
        {
            return await _repository.CountAsync();
        }

        public List<SelectListItem> GetEnumSelectList<T>()
        {
            var enumValues = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            var selectList = enumValues.Select((e, i) => new SelectListItem
            {
                Value = i.ToString(),
                Text = e.ToString()
            }).ToList();

            return selectList;
        }

        protected IEnumerable<TDto> MapToDtos(IEnumerable<TEntity> entities)
        {
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }

        public TEntity MapToEntity(TDto dto)
        {
            return _mapper.Map<TEntity>(dto);
        }
    }
}
