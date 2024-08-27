using AnimalAllies.Domain.Common;
using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Domain.Models.Volunteer.Pet;

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
    

    public static Result<HelpStatus> Create(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return Errors.General.ValueIsRequired(input);

        if(_all.Any(s => s.Value.ToLower() == input) == false)
        {
            return Errors.General.ValueIsInvalid(input);
        }

        return new HelpStatus(input);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
