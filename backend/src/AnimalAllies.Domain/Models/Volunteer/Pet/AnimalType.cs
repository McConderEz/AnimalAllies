using AnimalAllies.Domain.Common;
using AnimalAllies.Domain.Models.Species;
using AnimalAllies.Domain.Models.Species.Breed;
using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Domain.Models.Volunteer.Pet;

public class AnimalType: ValueObject
{
    public SpeciesId SpeciesId { get; }
    public Guid BreedId { get; }

    private AnimalType(){}

    public AnimalType(SpeciesId speciesId, Guid breedId)
    {
        SpeciesId = speciesId;
        BreedId = breedId;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return SpeciesId.Id;
        yield return BreedId;
    }
}