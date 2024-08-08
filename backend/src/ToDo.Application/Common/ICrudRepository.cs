namespace ToDo.Application.Common;

public interface ICrudRepository<T>
    where T: class
{
    Task CreateAsync(T entity, CancellationToken? cancellationToken);
    Task UpdateAsync(T entity, CancellationToken? cancellationToken);
    Task DeleteAsync(int id, CancellationToken? cancellationToken);
    Task<T> GetByIdAsync(int id, CancellationToken? cancellationToken);
    Task<List<T>> GetAsync(CancellationToken? cancellationToken);
}