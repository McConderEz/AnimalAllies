using AnimalAllies.SharedKernel.Shared;

namespace Discussion.Domain.ValueObjects;

public class IsEdited: ValueObject
{
    public bool Value { get; }
    
    private IsEdited(){}

    public IsEdited(bool value)
    {
        Value = value;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}