using System.Xml;

namespace AnimalAllies.Domain.Models;

public abstract class Entity
{
    public Guid Id { get; } = Guid.Empty;

    protected Entity() {}
    
    protected Entity(Guid id) => Id = id;
    
    public override bool Equals(object? obj)
    {
        if(obj is not Entity other)
            return false;
        
        if (!ReferenceEquals(this, other))
            return false;

        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
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