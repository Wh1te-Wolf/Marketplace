using Core.Entities;

namespace Core.Interfaces;

public interface IBaseRepository<T> where T : IEntity
{
    Task<T> AddAsync(T entity);

    Task<T> UpdateAsync(T entity);

    Task DeleteAsync(Guid uuid);

    Task<T?> GetAsync(Guid uuid, IEnumerable<string>? toInclude = null);

    Task<IReadOnlyCollection<T>> GetAsync(IEnumerable<Guid>? uuids, IEnumerable<string>? toInclude = null);

    Task<IReadOnlyCollection<T>> GetAllAsync(IEnumerable<string>? toInclude = null);

    Task<IReadOnlyCollection<T>> FindAsync(Filter? filter = null, string? orderBy = null, bool? sortDescending = null);
}
