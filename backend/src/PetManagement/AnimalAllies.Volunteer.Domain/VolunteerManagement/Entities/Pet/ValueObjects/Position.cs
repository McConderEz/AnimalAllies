

using AnimalAllies.SharedKernel.Shared;

namespace AnimalAllies.Volunteer.Domain.VolunteerManagement.Entities.Pet.ValueObjects;

public class Position : SharedKernel.Shared.ValueObject
{
    public static Position First = new(1);
    public int Value { get; }
    
    private Position(){}

    public Position(int value)
    {
        Value = value;
    }

    public Result<Position> Forward()
        => Create(Value + 1);
    
    public Result<Position> Back()
        => Create(Value - 1);
    
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