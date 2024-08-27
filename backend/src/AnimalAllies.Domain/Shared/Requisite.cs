using AnimalAllies.Domain.Models;
using Common_ValueObject = AnimalAllies.Domain.Common.ValueObject;
using ValueObject = AnimalAllies.Domain.Common.ValueObject;

namespace AnimalAllies.Domain.Shared;

public class Requisite : Common_ValueObject
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
            return Errors.General.ValueIsRequired(title);
        }
        
        if(string.IsNullOrWhiteSpace(description) || title.Length > Constraints.Constraints.MAX_VALUE_LENGTH)
        {
            return Errors.General.ValueIsRequired(description);
        }

        return new Requisite(title, description);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Title;
        yield return Description;
    }
}