using System.Text.RegularExpressions;
using AnimalAllies.SharedKernel.Shared.Objects;

namespace AnimalAllies.SharedKernel.Shared.ValueObjects;

public class Email: ValueObject
{
    public static readonly Regex ValidationRegex = new Regex(
        @"^[\w-\.]{1,40}@([\w-]+\.)+[\w-]{2,4}$",
        RegexOptions.Singleline | RegexOptions.Compiled);
    
    public string Value { get; }
    
    private Email(){}

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string email)
    {
        if (!ValidationRegex.IsMatch(email))
        {
            return Errors.Errors.General.ValueIsInvalid(email);
        }

        return new Email(email);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}