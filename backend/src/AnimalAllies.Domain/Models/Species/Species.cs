using AnimalAllies.Domain.Constraints;
using CSharpFunctionalExtensions;

namespace AnimalAllies.Domain.Models;

public class Species: Entity
{
    private readonly List<Breed> _breeds = [];
    private Species(){}
    private Species(string name, List<Breed> breeds)
    {
        Name = name;
        AddBreeds(breeds);
    }
    
    public string Name { get; private set; }
    public IReadOnlyList<Breed> Breeds => _breeds;
    public void AddBreeds(List<Breed> breeds) => _breeds.AddRange(breeds);

    public static Result<Species> Create(string name, List<Breed> breeds)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constraints.Constraints.MAX_VALUE_LENGTH)
        {
            return Result.Failure<Species>(
                $"{name} cannot be null or have length more than {Constraints.Constraints.MAX_VALUE_LENGTH}");
        }

        return Result.Success<Species>(new Species(name, breeds ?? []));
    }
    
}