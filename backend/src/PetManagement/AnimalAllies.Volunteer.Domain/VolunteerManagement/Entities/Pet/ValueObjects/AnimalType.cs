

using AnimalAllies.SharedKernel.Shared.Ids;

namespace AnimalAllies.Volunteer.Domain.VolunteerManagement.Entities.Pet.ValueObjects;

public class AnimalType: SharedKernel.Shared.ValueObject
{
    public SpeciesId SpeciesId { get; }
    public Guid BreedId { get; } = Guid.Empty;

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