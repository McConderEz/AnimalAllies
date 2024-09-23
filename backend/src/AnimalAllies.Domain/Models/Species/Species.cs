using AnimalAllies.Domain.Common;
using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Domain.Models.Species;

public class Species: Entity<SpeciesId>, ISoftDeletable
{
    private bool _isDeleted = false;
    private readonly List<Breed.Breed> _breeds = [];
    private Species(){}
    public Species(SpeciesId speciesId,Name name)
        : base(speciesId)
    {
        Name = name;
    }
    
    public Name Name { get; private set; }
    public IReadOnlyList<Breed.Breed> Breeds => _breeds;

    public Result AddBreed(Breed.Breed breed)
    {
        _breeds.Add(breed);

        return Result.Success();
    }

    public Result UpdateName(Name name)
    {
        Name = name;
        return Result.Success();
    }

    public void Delete() => _isDeleted = !_isDeleted;

}