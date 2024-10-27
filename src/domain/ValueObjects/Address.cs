namespace Shopzy.Domain.ValueObjects;

public class Address : ValueObject
{
    public string City { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public string Street { get; private set; } = string.Empty;
    public string ZipCode { get; private set; } = string.Empty;

    private Address()
    {
    }

    public static Address Create(string street, string city, string state, string zipcode, string country)
    {
        return new Address
        {
            City = city,
            Country = country,
            State = state,
            Street = street,
            ZipCode = zipcode
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return City;
        yield return Country;
        yield return State;
        yield return Street;
        yield return ZipCode; 
    }
}
