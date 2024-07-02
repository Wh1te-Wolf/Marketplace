using Core.Entities;

namespace RemoteRESTClients.Interfaces
{
    public interface IBaseRemoteService<T>
    {
        Task<T> GetAsync(Guid uuid);

        Task<IReadOnlyCollection<T>> GetAsync();

        Task<IReadOnlyCollection<T>> FindAsync(Filter? filter = null, string? orderBy = null, bool? sortDescending = null);
    }
}
