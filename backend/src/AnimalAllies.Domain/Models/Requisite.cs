using CSharpFunctionalExtensions;

namespace AnimalAllies.Domain.Models;

public class Requisite
{
    private Requisite(string title, string description)
    {
        Title = title;
        Description = description;
    }
    
    public string Title { get; } = string.Empty;
    public string Description { get; } = string.Empty;

    public static Result<Requisite> Create(string title, string description)
    {
        if(string.IsNullOrWhiteSpace(title) || title.Length > Constraints.Constraints.MAX_REQUISITE_TITLE_LENGTH)
        {
            return Result.Failure<Requisite>(
                $"{title} cannot be null or have length more than {Constraints.Constraints.MAX_REQUISITE_TITLE_LENGTH}");
        }
        
        if(string.IsNullOrWhiteSpace(description) || title.Length > Constraints.Constraints.MAX_REQUISITE_DESCRIPTION_LENGTH)
        {
            return Result.Failure<Requisite>(
                $"{description} cannot be null or have length more than {Constraints.Constraints.MAX_REQUISITE_DESCRIPTION_LENGTH}");
        }

        var requisite = new Requisite(title, description);

        return Result.Success(requisite);
    }

}