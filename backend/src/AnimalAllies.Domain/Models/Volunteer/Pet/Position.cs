using AnimalAllies.Domain.Common;
using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Domain.Models.Volunteer.Pet;

public class Position : ValueObject
{
    public static Position First = new(1);
    public int Value { get; }
    
    private Position(){}

    public Position(int value)
    {
        Value = value;
    }

    public static Result<Position> Create(int value)
    {
        if (value < 1)
            return Errors.General.ValueIsInvalid(nameof(value));

        return new Position(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}