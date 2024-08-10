using CSharpFunctionalExtensions;
using ValueObject = AnimalAllies.Domain.ValueObjects.ValueObject;

namespace AnimalAllies.Domain.ValueObjects;

public class AnimalType: ValueObject
{
    private static readonly AnimalType Cat = new(nameof(Cat));
    private static readonly AnimalType Dog = new(nameof(Dog));

    private static readonly AnimalType[] _all = [Cat, Dog];
    
    public string Value { get; }
    private AnimalType(){}
    private AnimalType(string value)
    {
        Value = value;
    }
    
    public static Result<AnimalType> Create(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return Result.Failure<AnimalType>($"{nameof(input)} cannot be null");

        var animalType = input.Trim().ToLower();

        if(_all.Any(s => s.Value.ToLower() == animalType) == false)
        {
            return Result.Failure<AnimalType>($"{animalType} is not correct");
        }

        return Result.Success(new AnimalType(input));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}