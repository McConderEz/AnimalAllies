using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using AnimalAllies.SharedKernel.Shared.ValueObjects;
using AnimalAllies.Species.Domain.Entities;

namespace AnimalAllies.Species.Domain;

public class Species: SoftDeletableEntity<SpeciesId>
{
    private readonly List<Breed> _breeds = [];
    private Species(SpeciesId id): base(id){}
    public Species(SpeciesId speciesId,Name name)
        : base(speciesId)
    {
        Name = name;
    }
    
    public Name Name { get; private set; }
    public IReadOnlyList<Breed> Breeds => _breeds;

    public Result AddBreed(Breed breed)
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

    public Result<Breed> GetById(BreedId id)
    {
        var breed = _breeds.FirstOrDefault(b => b.Id == id);

        if (breed == null)
            return Errors.General.NotFound();

        return breed;
    }

    public Result DeleteBreed(BreedId id, DateTime deletionTime)
    {
        var breed = GetById(id);
        if (breed.IsFailure)
            return Errors.General.NotFound();

        _breeds.Remove(breed.Value);
        breed.Value.Delete(deletionTime);
        
        return Result.Success();
    }

    public override void Delete(DateTime deletionTime)
    {
        base.Delete(deletionTime);
        _breeds.ForEach(b => b.Delete(deletionTime));
    }

    public override void Restore()
    {
        base.Restore();
        _breeds.ForEach(b => b.Restore());
    }
    
    public void DeleteExpiredBreeds(int expiredTime)
    {
        _breeds.RemoveAll(p => p.DeletionDate != null 
                             && p.DeletionDate.Value.AddDays(expiredTime) <= DateTime.UtcNow);
    }
}