using AnimalAllies.Domain.Constraints;
using AnimalAllies.Domain.Shared;
using AnimalAllies.Domain.ValueObjects;

namespace AnimalAllies.Domain.Models;

public class Species: Entity<SpeciesId>
{
    private readonly List<Breed> _breeds = [];
    private Species(){}
    private Species(SpeciesId speciesId,Name name, List<Breed> breeds)
        : base(speciesId)
    {
        Name = name;
        AddBreeds(breeds);
    }
    
    public Name Name { get; private set; }
    public IReadOnlyList<Breed> Breeds => _breeds;
    public void AddBreeds(List<Breed> breeds) => _breeds.AddRange(breeds);

    public Result UpdateName(Name name)
    {
        Name = name;
        return Result.Success();
    }

    public static Result<Species> Create(SpeciesId speciesId,string name, List<Breed> breeds)
    {
        var nameVo = Name.Create(name);

        if (nameVo.IsFailure)
        {
            return Result<Species>.Failure(nameVo.Error);
        }

        return Result<Species>.Success(new Species(speciesId,nameVo.Value, breeds ?? []));
    }
    
}