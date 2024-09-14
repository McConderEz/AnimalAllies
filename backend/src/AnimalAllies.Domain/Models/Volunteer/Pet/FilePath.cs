using AnimalAllies.Domain.Common;
using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Domain.Models.Volunteer.Pet;

public class FilePath: ValueObject
{
    public string Path { get; }
    
    private FilePath(){}

    private FilePath(string path)
    {
        Path = path;
    }

    public static Result<FilePath> Create(Guid path, string extension)
    {
        //TODO: Сделать валидацию на доступные расширения
        
        if(string.IsNullOrWhiteSpace(extension) || extension.Length > Constraints.Constraints.MAX_VALUE_LENGTH)
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