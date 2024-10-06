

using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared;

namespace AnimalAllies.Volunteer.Domain.VolunteerManagement.ValueObject;

public class WorkExperience : SharedKernel.Shared.ValueObject
{
    public int Value { get; }
    
    private WorkExperience(){}

    private WorkExperience(int value)
    {
        Value = value;
    }
    
    public static Result<WorkExperience> Create(int workExperience)
    {
        if (workExperience < 0 || workExperience > Constraints.MAX_EXP_VALUE)
            return Errors.General.ValueIsInvalid(nameof(workExperience));

        return Result<WorkExperience>.Success(new WorkExperience(workExperience));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}