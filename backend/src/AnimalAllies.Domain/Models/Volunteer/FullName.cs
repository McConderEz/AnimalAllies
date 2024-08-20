using System.Text.RegularExpressions;
using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.Shared;


namespace AnimalAllies.Domain.ValueObjects;

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
            Errors.General.ValueIsInvalid(firstName);

        if (string.IsNullOrWhiteSpace(secondName) || !ValidationRegex.IsMatch(secondName))
            Errors.General.ValueIsInvalid(secondName);

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