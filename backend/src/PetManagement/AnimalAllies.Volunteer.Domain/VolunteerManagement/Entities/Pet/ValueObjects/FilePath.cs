

using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared;

namespace AnimalAllies.Volunteer.Domain.VolunteerManagement.Entities.Pet.ValueObjects;

public class FilePath: SharedKernel.Shared.ValueObject
{
    public string Path { get; }
    
    private FilePath(){}

    private FilePath(string path)
    {
        Path = path;
    }

    public static Result<FilePath> Create(Guid path, string extension)
    {
        if (!Constraints.Extensions.Contains(extension))
            return Errors.General.ValueIsInvalid("extension");
        
        if(string.IsNullOrWhiteSpace(extension) || extension.Length > Constraints.MAX_VALUE_LENGTH)
            return Errors.General.ValueIsRequired(extension);

        var fullPath = path.ToString() + extension; 
        
        return new FilePath(fullPath);
    }
    
    public static Result<FilePath> Create(string fullPath)
    {
        return new FilePath(fullPath);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Path;
    }
}