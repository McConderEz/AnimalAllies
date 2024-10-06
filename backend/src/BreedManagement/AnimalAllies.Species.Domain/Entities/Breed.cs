

using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using AnimalAllies.SharedKernel.Shared.ValueObjects;

namespace AnimalAllies.Species.Domain.Entities;

public class Breed: Entity<BreedId>, ISoftDeletable
{
    private bool _isDeleted = false;
    private Breed(){}
    public Breed(BreedId breedId, Name name) : base(breedId)
    {
        Name = name;
    }
    
    public Name Name { get; private set; }

    public Result UpdateName(Name name)
    {
        Name = name;
        return Result.Success();
    }

    public void Delete() => _isDeleted = !_isDeleted;
}