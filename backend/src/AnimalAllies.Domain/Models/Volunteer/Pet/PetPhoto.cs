using AnimalAllies.Domain.Common;
using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Domain.Models.Volunteer.Pet;

public class PetPhoto: ValueObject
{
    public FilePath Path { get; }
    public bool IsMain { get; }
    
    private PetPhoto(){}
    public PetPhoto(FilePath path, bool isMain)
    {
        Path = path;
        IsMain = isMain;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Path;
        yield return IsMain;
    }
}