namespace AnimalAllies.Application.Common;

public interface ICrudService<T>
    where T: notnull
{
    Task Create(T entity);
    Task Delete(Guid id);
    Task Update(T entity);
    Task<T> GetById(Guid id);
    Task<List<T>> Get();
}