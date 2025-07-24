namespace Domain.ValueObjects;

public abstract class UserLocation
{
    public string Country { get; } 
    public string City { get; }

    protected UserLocation(string country, string city)
    {
        if (string.IsNullOrWhiteSpace(country) || string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("Country and City are required.");

        Country = country;
        City = city;
    }
}

// todo - Implement on Domain