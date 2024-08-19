
using AnimalAllies.Domain.Shared;
using AnimalAllies.Domain.ValueObjects;
using ValueObject = AnimalAllies.Domain.Shared.ValueObject;

namespace AnimalAllies.Domain.Models;

public class Breed: Entity<BreedId>
{
    private Breed(){}
    private Breed(BreedId breedId, Name name) : base(breedId)
    {
        Name = name;
    }
    
    public Name Name { get; private set; }

    public Result UpdateName(Name name)
    {
        Name = name;
        return Result.Success();
    }
    
    public static Result<Breed> Create(BreedId breedId,string name)
    {
        var nameVo = Name.Create(name);

        if (nameVo.IsFailure)
        {
            return Result<Breed>.Failure(nameVo.Error);
        }
        return Result<Breed>.Success(new Breed(breedId, nameVo.Value));
    }
}