
using AnimalAllies.Domain.Models;

namespace AnimalAllies.Domain.ValueObjects;

public class HelpStatus:ValueObject
{
    public static readonly HelpStatus NeedsHelp = new(nameof(NeedsHelp));
    public static readonly HelpStatus SearchingHome = new(nameof(SearchingHome));
    public static readonly HelpStatus FoundHome = new(nameof(FoundHome));

    
    private static readonly HelpStatus[] _all = [NeedsHelp, SearchingHome, FoundHome];

    public string Value { get; }

    private HelpStatus(){}
    private HelpStatus(string value)
    {
        Value = value;
    }

    public static Result<HelpStatus> Update(string newValue)
    {
        var result = Create(newValue);

        if (result.IsSuccess)
        {
            return result.Value;
        }

        return result;
    }

    public static Result<HelpStatus> Create(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return Result<HelpStatus>.Failure(new Error("Invalid input",$"{nameof(input)} cannot be null"));

        var status = input.Trim().ToLower();

        if(_all.Any(s => s.Value.ToLower() == status) == false)
        {
            return Result<HelpStatus>.Failure(new Error("Invalid input",$"{status} is not correct"));
        }

        return Result<HelpStatus>.Success(new HelpStatus(input));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
