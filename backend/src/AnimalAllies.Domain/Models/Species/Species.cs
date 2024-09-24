using AnimalAllies.Domain.Common;
using AnimalAllies.Domain.Models.Species.Breed;
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
        var breedAlreadyExist = _breeds.FirstOrDefault(b => b.Name == breed.Name);
        if (breedAlreadyExist is not null)
            return Errors.Species.BreedAlreadyExist();
        
        _breeds.Add(breed);

        return Result.Success();
    }

    public Result UpdateName(Name name)
    {
        Name = name;
        return Result.Success();
    }

    public Result<Breed.Breed> GetById(BreedId id)
    {
        var breed = _breeds.FirstOrDefault(b => b.Id == id);

        if (breed == null)
            return Errors.General.NotFound();

        return breed;
    }

    public Result DeleteBreed(BreedId id)
    {
        var breed = GetById(id);
        if (breed.IsFailure)
            return Errors.General.NotFound();
        
        breed.Value.Delete();

        return Result.Success();
    }
    
    public void Delete() => _isDeleted = !_isDeleted;

}