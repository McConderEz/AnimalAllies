using AnimalAllies.Domain.Models.Common;
using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Domain.Models.Volunteer.Pet;

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
            return Errors.General.ValueIsRequired(description);
        }
        
        if (birthDate > DateOnly.FromDateTime(DateTime.Now))
        {
            return Errors.General.ValueIsInvalid(nameof(birthDate));
        }

        return new PetDetails(description, birthDate, creationTime);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Description;
        yield return BirthDate;
        yield return CreationTime;
    }
}