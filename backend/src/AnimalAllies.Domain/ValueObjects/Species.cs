using AnimalAllies.Domain.Constraints;
using CSharpFunctionalExtensions;

namespace AnimalAllies.Domain.ValueObjects;

public class Species: ValueObject
{
    
    public string Value { get; }
    
    private Species(string value)
    {
        Value = value;
    }
    

    public static Result<Species> Create(string input)
    {
        if (string.IsNullOrWhiteSpace(input) || input.Length > Constraints.Constraints.MAX_VALUE_LENGTH)
        {
            return Result.Failure<Species>(
                $"{input} cannot be null or have length more than {Constraints.Constraints.MAX_VALUE_LENGTH}");
        }

        return Result.Success<Species>(new Species(input));
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}