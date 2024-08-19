

using AnimalAllies.Domain.Shared;
using AnimalAllies.Domain.ValueObjects;

namespace AnimalAllies.Domain.Models;

public class PetPhoto: ValueObject
{
    public string Path { get; }
    public bool IsMain { get; }
    
    private PetPhoto(){}
    private PetPhoto(string path, bool isMain)
    {
        Path = path;
        IsMain = isMain;
    }

    public static Result<PetPhoto> Create(string path, bool isMain)
    {
        if (string.IsNullOrWhiteSpace(path) || path.Length > Constraints.Constraints.MAX_PATH_LENGHT)
        {
            return Result<PetPhoto>.Failure(Errors.General.ValueIsRequired(path));
        }

        return Result<PetPhoto>.Success(new PetPhoto(path, isMain));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Path;
        yield return IsMain;
    }
}