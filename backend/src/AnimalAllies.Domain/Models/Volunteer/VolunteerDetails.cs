using AnimalAllies.Domain.ValueObjects;

namespace AnimalAllies.Domain.Models;

public class VolunteerDetails: ValueObject
{
    public string Description { get; }
    public int WorkExperience { get; }

    private VolunteerDetails(){}
    
    private VolunteerDetails(string description, int workExperience)
    {
        Description = description;
        WorkExperience = workExperience;
    }

    public static Result<VolunteerDetails> Create(string description, int workExperience)
    {
        if (string.IsNullOrWhiteSpace(description) ||
            description.Length > Constraints.Constraints.MAX_DESCRIPTION_LENGTH)
        {
            return Result<VolunteerDetails>.Failure(new Error("Invalid input",
                $"description cannot be null or have length more than {Constraints.Constraints.MAX_DESCRIPTION_LENGTH}"));
        }

        if (workExperience < 0 || workExperience > Constraints.Constraints.MAX_EXP_VALUE)
        {
            return Result<VolunteerDetails>.Failure(new Error("Invalid input",
                $"workExp cannot be less than 0 or more than {Constraints.Constraints.MAX_EXP_VALUE}"));
        }

        return Result<VolunteerDetails>.Success(new VolunteerDetails(description, workExperience));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Description;
        yield return WorkExperience;
    }
}