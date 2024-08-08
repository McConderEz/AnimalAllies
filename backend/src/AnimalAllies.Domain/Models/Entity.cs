using System.Xml;

namespace AnimalAllies.Domain.Models;

public abstract class Entity
{
    public int Id { get; }

    protected Entity(int id) => Id = id;
}