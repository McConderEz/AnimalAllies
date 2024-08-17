
using ValueObject = AnimalAllies.Domain.ValueObjects.ValueObject;

namespace AnimalAllies.Domain.Models;

public class Breed: Entity<BreedId>
{
    private Breed(){}
    private Breed(BreedId breedId, string name) : base(breedId)
    {
        Name = name;
    }
    
    public string Name { get; private set; }

    public Result UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constraints.Constraints.MAX_VALUE_LENGTH)
            return Result.Failure(new Error("Invalid input",$"{nameof(name)} cannot be null or length more than {Constraints.Constraints.MAX_VALUE_LENGTH}"));

        Name = name;
        return Result.Success();
    }
    
    public static Result<Breed> Create(BreedId breedId,string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constraints.Constraints.MAX_VALUE_LENGTH)
            return Result<Breed>.Failure(new Error("Invalid input",$"{nameof(name)} cannot be null or length more than {Constraints.Constraints.MAX_VALUE_LENGTH}"));

        var breed = new Breed(breedId,name);

        return Result<Breed>.Success(breed);
    }
}