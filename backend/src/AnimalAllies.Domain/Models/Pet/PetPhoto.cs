

namespace AnimalAllies.Domain.Models;

public class PetPhoto: Entity<PetPhotoId>
{
    private PetPhoto(PetPhotoId id) : base(id) {}
    private PetPhoto(PetPhotoId petPhotoId,string path, bool isMain)
    :base(petPhotoId)
    {
        Path = path;
        IsMain = isMain;
    }

    public string Path { get; private set; }
    public bool IsMain { get; private set; } = false;

    public void SetMain() => IsMain = !IsMain;

    public Result UpdatePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path) || path.Length > Constraints.Constraints.MAX_PATH_LENGHT)
        {
            return Result.Failure(Errors.General.ValueIsRequired(path));
        }

        Path = path;
        return Result.Success();
    }

    public static Result<PetPhoto> Create(PetPhotoId petPhotoId,string path, bool isMain)
    {
        if (string.IsNullOrWhiteSpace(path) || path.Length > Constraints.Constraints.MAX_PATH_LENGHT)
        {
            return Result<PetPhoto>.Failure(Errors.General.ValueIsRequired(path));
        }

        return Result<PetPhoto>.Success(new PetPhoto(petPhotoId,path, isMain));
    }

}