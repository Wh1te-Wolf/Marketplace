using Core.Entities;

namespace Core.Interfaces;

public interface IBaseService<T> where T : IEntity
{
    Task<T> AddAsync(T entity);

    Task<T> UpdateAsync(T entity);

    Task DeleteAsync(Guid uuid);

    Task<T> GetAsync(Guid uuid);

    Task<IReadOnlyCollection<T>> GetAsync(IEnumerable<Guid>? uuids);

    Task<IReadOnlyCollection<T>> GetAsync();

    Task<IReadOnlyCollection<T>> FindAsync(Filter? filter = null, string? orderBy = null, bool? sortDescending = null);
}
