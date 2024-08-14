
using AnimalAllies.Domain.Models;
using ValueObject = AnimalAllies.Domain.ValueObjects.ValueObject;

namespace AnimalAllies.Domain.ValueObjects;

public class Requisite : ValueObject
{
    
    public string Title { get; } 
    public string Description { get; }
    
    private Requisite(){}
    private Requisite(string title, string description)
    {
        Title = title;
        Description = description;
    }
    

    public static Result<Requisite> Create(string title, string description)
    {
        if(string.IsNullOrWhiteSpace(title) || title.Length > Constraints.Constraints.MAX_VALUE_LENGTH)
        {
            return Result<Requisite>.Failure(new Error("Invalid input",
                $"{title} cannot be null or have length more than {Constraints.Constraints.MAX_VALUE_LENGTH}"));
        }
        
        if(string.IsNullOrWhiteSpace(description) || title.Length > Constraints.Constraints.MAX_VALUE_LENGTH)
        {
            return Result<Requisite>.Failure(new Error("Invalid input",
                $"{description} cannot be null or have length more than {Constraints.Constraints.MAX_VALUE_LENGTH}"));
        }

        var requisite = new Requisite(title, description);

        return Result<Requisite>.Success(requisite);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Title;
        yield return Description;
    }
}