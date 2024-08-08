using CSharpFunctionalExtensions;
using ValueObject = AnimalAllies.Domain.ValueObjects.ValueObject;

namespace AnimalAllies.Domain.ValueObjects;

public class Requisite : ValueObject
{
    
    public string Title { get; } = string.Empty;
    public string Description { get; } = string.Empty;
    
    private Requisite(string title, string description)
    {
        Title = title;
        Description = description;
    }
    

    public static Result<Requisite> Create(string title, string description)
    {
        if(string.IsNullOrWhiteSpace(title) || title.Length > Constraints.Constraints.MAX_VALUE_LENGTH)
        {
            return Result.Failure<Requisite>(
                $"{title} cannot be null or have length more than {Constraints.Constraints.MAX_VALUE_LENGTH}");
        }
        
        if(string.IsNullOrWhiteSpace(description) || title.Length > Constraints.Constraints.MAX_VALUE_LENGTH)
        {
            return Result.Failure<Requisite>(
                $"{description} cannot be null or have length more than {Constraints.Constraints.MAX_VALUE_LENGTH}");
        }

        var requisite = new Requisite(title, description);

        return Result.Success(requisite);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Title;
        yield return Description;
    }
}