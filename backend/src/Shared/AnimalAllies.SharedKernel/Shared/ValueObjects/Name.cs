using AnimalAllies.SharedKernel.Shared.Objects;

namespace AnimalAllies.SharedKernel.Shared.ValueObjects;

public class Name : ValueObject
{
    public string Value { get; }
    
    private Name(){}

    private Name(string value)
    {
        Value = value;
    }

    public static Result<Name> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > Constraints.Constraints.MAX_VALUE_LENGTH)
        {
            return Errors.Errors.General.ValueIsRequired(value);
        }

        return Result<Name>.Success(new Name(value));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}