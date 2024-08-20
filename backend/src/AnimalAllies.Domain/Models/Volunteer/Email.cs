using System.Text.RegularExpressions;
using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Domain.ValueObjects;

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
            return Errors.General.ValueIsInvalid(email);
        }

        return new Email(email);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}