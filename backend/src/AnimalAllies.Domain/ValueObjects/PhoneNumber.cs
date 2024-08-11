using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace AnimalAllies.Domain.ValueObjects;

public class PhoneNumber : ValueObject
{
    private static readonly Regex ValidationRegex = new Regex(
        @"(^\+\d{1,3}\d{10}$|^$)",
        RegexOptions.Singleline | RegexOptions.Compiled);

    public string Number { get; }

    private PhoneNumber(){}
    private PhoneNumber(string number)
    {
        Number = number;
    }

    public static Result<PhoneNumber> Create(string number)
    {
        if (string.IsNullOrWhiteSpace(number) || !ValidationRegex.IsMatch(number))
            return Result.Failure<PhoneNumber>($"{nameof(number)} incorrect format");

        var phoneNumber = new PhoneNumber(number);

        return Result.Success(phoneNumber);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number;
    }
}