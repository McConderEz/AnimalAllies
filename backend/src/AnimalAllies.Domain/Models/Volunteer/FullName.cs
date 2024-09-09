using System.Text.RegularExpressions;
using AnimalAllies.Domain.Common;
using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Domain.Models.Volunteer;

public class FullName : ValueObject
{

    private static readonly Regex ValidationRegex = new Regex(
        @"^[\p{L}\p{M}\p{N}]{1,50}\z",
        RegexOptions.Singleline | RegexOptions.Compiled);

    public string FirstName { get; }
    public string SecondName { get; }
    public string? Patronymic { get; } 

    private FullName(){}
    private FullName(string firstName, string secondName, string? patronymic)
    {
        FirstName = firstName;
        SecondName = secondName;
        Patronymic = patronymic;
    }
    
    
    public static Result<FullName> Create(string firstName,string secondName, string? patronymic)
    {
        if (string.IsNullOrWhiteSpace(firstName) || !ValidationRegex.IsMatch(firstName))
            return Errors.General.ValueIsInvalid(firstName);

        if (string.IsNullOrWhiteSpace(secondName) || !ValidationRegex.IsMatch(secondName))
            return Errors.General.ValueIsInvalid(secondName);

        return new FullName(firstName, secondName, patronymic);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return SecondName;
        yield return Patronymic;
    }

    public override string ToString()
    {
        return $"{SecondName} {FirstName} {Patronymic}";
    }
}