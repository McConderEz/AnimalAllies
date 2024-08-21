using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Domain.Models.Volunteer.Pet;

public class Address : ValueObject
{
    public string Street { get; }
    public string City { get; }
    public string State { get; }
    public string ZipCode { get; }

    private Address(){}
    private Address(string street, string city, string state, string zipCode)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
    }
    
    
    public static Result<Address> Create(string street, string city, string state, string zipCode)
    {
        if (string.IsNullOrWhiteSpace(street) || street.Length > Constraints.Constraints.MAX_VALUE_LENGTH)
            return Errors.General.ValueIsInvalid($"Street cannot be empty or more then {Constraints.Constraints.MAX_VALUE_LENGTH}.");

        if (string.IsNullOrWhiteSpace(city) || city.Length > Constraints.Constraints.MAX_VALUE_LENGTH)
            return Errors.General.ValueIsInvalid($"City cannot be empty or more then {Constraints.Constraints.MAX_VALUE_LENGTH}.");

        if (string.IsNullOrWhiteSpace(state) || state.Length > Constraints.Constraints.MAX_VALUE_LENGTH)
            return Errors.General.ValueIsInvalid($"State cannot be empty or more then {Constraints.Constraints.MAX_VALUE_LENGTH}.");

        if (string.IsNullOrWhiteSpace(zipCode) || zipCode.Length > Constraints.Constraints.MAX_VALUE_LENGTH)
            return Errors.General.ValueIsInvalid($"ZipCode cannot be empty or more then {Constraints.Constraints.MAX_VALUE_LENGTH}.");

        return new Address(street, city, state, zipCode);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return State;
        yield return ZipCode;
    }
}

    