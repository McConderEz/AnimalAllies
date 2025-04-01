using AnimalAllies.SharedKernel.Shared.Ids;

namespace AnimalAllies.SharedKernel.Shared.Objects;

/// <summary>
/// Абстрактный класс для реализации сущности с доменными событиями
/// </summary>
/// <typeparam name="TId">Обобщенный класс Id</typeparam>
public abstract class DomainEntity<TId>: Entity<TId>
    where TId: BaseId<TId>
{
    //Ef core configuration
    protected DomainEntity(TId id) : base(id){}

    private readonly Queue<IDomainEvent> _domainEvents = [];

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.ToList();
    
    /// <summary>
    /// Добавить событие в очередь
    /// </summary>
    /// <param name="event">доменное событие</param>
    protected void AddDomainEvent(IDomainEvent @event) => _domainEvents.Enqueue(@event);

    /// <summary>
    /// Удаление события из очереди
    /// </summary>
    /// <param name="event">доменное событие</param>
    public void RemoveDomainEvent(IDomainEvent @event)
        => _domainEvents.TryDequeue(out var domainEvent);

    /// <summary>
    /// Очистка всех событий в очереди
    /// </summary>
    public void ClearDomainEvents() => _domainEvents.Clear();
}