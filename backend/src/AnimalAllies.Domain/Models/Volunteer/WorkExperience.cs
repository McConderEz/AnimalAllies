using AnimalAllies.Domain.Shared;
using AnimalAllies.Domain.ValueObjects;

namespace AnimalAllies.Domain.Models.Volunteer;

public class WorkExperience : ValueObject
{
    public int Value { get; }
    
    private WorkExperience(){}

    private WorkExperience(int value)
    {
        Value = value;
    }
    
    public static Result<WorkExperience> Create(int workExperience)
    {
        if (workExperience < 0 || workExperience > Constraints.Constraints.MAX_EXP_VALUE)
            return Result<WorkExperience>.Failure(Errors.General.ValueIsInvalid(nameof(workExperience)));

        return Result<WorkExperience>.Success(new WorkExperience(workExperience));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}