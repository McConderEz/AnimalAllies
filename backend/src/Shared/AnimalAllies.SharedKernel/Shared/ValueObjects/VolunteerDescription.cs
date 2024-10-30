namespace AnimalAllies.SharedKernel.Shared.ValueObjects;

public class VolunteerDescription : SharedKernel.Shared.ValueObject
{
    public string Value { get; }

    private VolunteerDescription(){}

    private VolunteerDescription(string value)
    {
        Value = value;
    }

    public static Result<VolunteerDescription> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > Constraints.Constraints.MAX_DESCRIPTION_LENGTH)
        {
            return Errors.General.ValueIsRequired(value);
        }

        return new VolunteerDescription(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}