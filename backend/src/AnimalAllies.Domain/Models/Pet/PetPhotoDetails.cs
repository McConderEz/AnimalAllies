using AnimalAllies.Domain.Shared;
using AnimalAllies.Domain.ValueObjects;

namespace AnimalAllies.Domain.Models.Pet;

public class PetPhotoDetails: ValueObject
{
    public IReadOnlyList<PetPhoto> PetPhotos { get; }
    
    private PetPhotoDetails(){}

    public PetPhotoDetails(IEnumerable<PetPhoto> petPhotos)
    {
        PetPhotos = petPhotos.ToList();
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return PetPhotos;
    }
}