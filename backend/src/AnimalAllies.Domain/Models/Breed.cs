using CSharpFunctionalExtensions;
using ValueObject = AnimalAllies.Domain.ValueObjects.ValueObject;

namespace AnimalAllies.Domain.Models;

public class Breed: Entity
{
    private Breed(){}
    private Breed(string name)
    {
        Name = name;
    }
    
    public string Name { get; private set; }
    
    
    public static Result<Breed> Create(string name, List<Species> speciesList)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constraints.Constraints.MAX_VALUE_LENGTH)
            return Result.Failure<Breed>($"{nameof(name)} cannot be null or length more than {Constraints.Constraints.MAX_VALUE_LENGTH}");

        var breed = new Breed(name);

        return Result.Success(breed);
    }
}