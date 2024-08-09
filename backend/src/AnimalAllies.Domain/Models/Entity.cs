using System.Xml;

namespace AnimalAllies.Domain.Models;

public abstract class Entity
{
    public Guid Id { get; } = Guid.Empty;

    protected Entity() {}
    
    protected Entity(Guid id) => Id = id;
}