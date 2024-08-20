using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Domain.Models.Species.Breed;

public class Breed: Entity<BreedId>
{
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
    
}