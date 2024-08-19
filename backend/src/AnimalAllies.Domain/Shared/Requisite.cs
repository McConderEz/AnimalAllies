using AnimalAllies.Domain.Models;
using ValueObject = AnimalAllies.Domain.Shared.ValueObject;

namespace AnimalAllies.Domain.Shared;

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
            return Result<Requisite>.Failure(Errors.General.ValueIsRequired(title));
        }
        
        if(string.IsNullOrWhiteSpace(description) || title.Length > Constraints.Constraints.MAX_VALUE_LENGTH)
        {
            return Result<Requisite>.Failure(Errors.General.ValueIsRequired(description));
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