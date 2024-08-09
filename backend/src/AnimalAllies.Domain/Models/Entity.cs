using System.Xml;

namespace AnimalAllies.Domain.Models;

public abstract class Entity
{
    public Guid Id { get; } = Guid.NewGuid();

    protected Entity() {}
    
    protected Entity(Guid id) => Id = id;
    
    public override bool Equals(object? obj)
    {
        if(obj == null)
            return false;

        if (GetType() != obj.GetType())
            return false;

        var entity = (Entity)obj;
        
        if (!ReferenceEquals(this, entity))
            return false;

        return Id == entity.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(Entity? a, Entity? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity a, Entity b) => !(a == b);
}