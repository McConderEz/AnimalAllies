using CSharpFunctionalExtensions;

namespace AnimalAllies.Domain.Models;

public class PetPhoto: Entity
{
    private PetPhoto(){}
    private PetPhoto(string path, bool isMain)
    {
        Path = path;
        IsMain = isMain;
    }

    public string Path { get; } = string.Empty;
    public bool IsMain { get; } = false;

    public static Result<PetPhoto> Create(string path, bool isMain)
    {
        if (string.IsNullOrWhiteSpace(path) || path.Length > Constraints.Constraints.MAX_PATH_LENGHT)
        {
            return Result.Failure<PetPhoto>(
                $"{path} cannot be null or have length more than {Constraints.Constraints.MAX_PATH_LENGHT}");
        }

        return Result.Success(new PetPhoto(path, isMain));
    }

}