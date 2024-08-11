using CSharpFunctionalExtensions;

namespace AnimalAllies.Domain.ValueObjects;

public class Address : ValueObject
{
    public string City { get; }
    public string District { get; }
    public int HouseNumber { get; }
    public int FlatNumber { get; }

    private Address(){}
    private Address(string city, string district, int houseNumber,int flatNumber)
    {
        City = city;
        District = district;
        HouseNumber = houseNumber;
        FlatNumber = flatNumber;
    }

    public static Result<Address> Create(string city, string district, int houseNumber, int flatNumber)
    {
        if (string.IsNullOrWhiteSpace(city) || city.Length > Constraints.Constraints.MAX_VALUE_LENGTH)
            return Result.Failure<Address>($"{nameof(city)} incorrect format");

        if (string.IsNullOrWhiteSpace(district) || district.Length > Constraints.Constraints.MAX_VALUE_LENGTH)
            return Result.Failure<Address>($"{nameof(district)} incorrect format");

        var address = new Address(city, district, houseNumber, flatNumber);

        return Result.Success(address);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return City;
        yield return District;
        yield return HouseNumber;
        yield return FlatNumber;
    }
}