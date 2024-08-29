using AnimalAllies.Domain.Models.Common;
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
    public void AddBreeds(List<Breed.Breed> breeds) => _breeds.AddRange(breeds);

    public Result UpdateName(Name name)
    {
        Name = name;
        return Result.Success();
    }

    public void Delete() => _isDeleted = !_isDeleted;

}