using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared;

namespace VolunteerRequests.Domain.ValueObjects;

public class RejectionComment : ValueObject
{
    public string Value { get; }
    
    private RejectionComment() {}

    private RejectionComment(string value)
    {
        Value = value;
    }

    public static Result<RejectionComment> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) && value.Length > Constraints.MAX_DESCRIPTION_LENGTH)
            return Errors.General.ValueIsRequired("rejection comment");

        return new RejectionComment(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}