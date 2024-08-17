using AnimalAllies.Domain.Constraints;

namespace AnimalAllies.Domain.Models;

public class Species: Entity<SpeciesId>
{
    private readonly List<Breed> _breeds = [];
    private Species(){}
    private Species(SpeciesId speciesId,string name, List<Breed> breeds)
        : base(speciesId)
    {
        Name = name;
        AddBreeds(breeds);
    }
    
    public string Name { get; private set; }
    public IReadOnlyList<Breed> Breeds => _breeds;
    public void AddBreeds(List<Breed> breeds) => _breeds.AddRange(breeds);

    public Result UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constraints.Constraints.MAX_VALUE_LENGTH)
        {
            return Result.Failure(new Error("Invalid input",
                $"{name} cannot be null or have length more than {Constraints.Constraints.MAX_VALUE_LENGTH}"));
        }

        Name = name;
        return Result.Success();
    }

    public static Result<Species> Create(SpeciesId speciesId,string name, List<Breed> breeds)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constraints.Constraints.MAX_VALUE_LENGTH)
        {
            return Result<Species>.Failure(new Error("Invalid input",
                $"{name} cannot be null or have length more than {Constraints.Constraints.MAX_VALUE_LENGTH}"));
        }

        return Result<Species>.Success(new Species(speciesId,name, breeds ?? []));
    }
    
}