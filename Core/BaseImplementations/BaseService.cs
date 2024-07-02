using Core.Entities;
using Core.Interfaces;

namespace Core.BaseImplementations;

public class BaseService<T> : IBaseService<T> where T : class, IEntity
{
    private readonly IBaseRepository<T> _baseRepository;

    public BaseService(IBaseRepository<T> baseRepository)
    {
        _baseRepository = baseRepository;
    }

    public virtual Task<T> AddAsync(T entity)
    {
        return _baseRepository.AddAsync(entity);
    }

    public virtual Task DeleteAsync(Guid uuid)
    {
        return _baseRepository.DeleteAsync(uuid);
    }

    public Task<IReadOnlyCollection<T>> FindAsync(Filter? filter = null, string? orderBy = null, bool? sortDescending = null)
    {
       return _baseRepository.FindAsync(filter, orderBy, sortDescending);
    }

    public virtual Task<T> GetAsync(Guid uuid)
    {
        return _baseRepository.GetAsync(uuid);
    }

    public virtual Task<IReadOnlyCollection<T>> GetAsync(IEnumerable<Guid>? uuids)
    {
        return _baseRepository.GetAsync(uuids);
    }

    public Task<IReadOnlyCollection<T>> GetAsync()
    {
        return _baseRepository.GetAllAsync();
    }

    public virtual Task<T> UpdateAsync(T entity)
    {
        return _baseRepository.UpdateAsync(entity);
    }
}
