using AnimalAllies.Domain.Common;
using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Domain.Models.Volunteer.Pet;

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