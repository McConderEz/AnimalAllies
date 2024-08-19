using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Domain.ValueObjects;

public class PetDetails: ValueObject
{
    public string Description { get; }
    public DateOnly BirthDate { get; } 
    public DateTime CreationTime { get; }
    
    private PetDetails(){}

    private PetDetails(string description, DateOnly birthDate, DateTime creationTime)
    {
        Description = description;
        BirthDate = birthDate;
        CreationTime = creationTime;
    }

    public static Result<PetDetails> Create(string description, DateOnly birthDate, DateTime creationTime)
    {
        if (string.IsNullOrWhiteSpace(description) ||
            description.Length > Constraints.Constraints.MAX_DESCRIPTION_LENGTH)
        {
            return Result<PetDetails>.Failure(Errors.General.ValueIsRequired(description));
        }
        
        if (birthDate > DateOnly.FromDateTime(DateTime.Now))
        {
            return Result<PetDetails>.Failure(Errors.General.ValueIsInvalid(nameof(birthDate)));
        }

        return Result<PetDetails>.Success(new PetDetails(description, birthDate, creationTime));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Description;
        yield return BirthDate;
        yield return CreationTime;
    }
}