namespace Domain.Entities;

public enum SexualOrientations
{
    Heterosexual = 0,
    Homosexual = 1,
    Bisexual = 2,
}

public enum Sexualities
{
    Female = 0,
    Male = 1,
}

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string? Name { get; private set; }
    public DateTime? Birthdate { get; private set; }
    public string? Country { get; private set; }
    public string? City { get; private set; }
    public Sexualities? Sexuality { get; private set; }
    public SexualOrientations? SexualOrientation { get; private set; }
    public string? Password { get; private set; }
    public double? Height { get; private set; }
    public double? Weight { get; private set; }
    public double? Latitude { get; private set; }
    public double? Longitude { get; private set; }

    public static User Rehydrate(
        Guid id,
        string email,
        string? name,
        DateTime? birthdate,
        string? country,
        string? city,
        Sexualities? sexuality,
        SexualOrientations? sexualOrientation,
        string? password,
        double? height,
        double? weight,
        double? latitude,
        double? longitude
        )
    {
        return new User(id, email, name, birthdate, country, city, sexuality, sexualOrientation, password, height, weight, latitude, longitude);
    }

    private User(
        Guid id,
        string email,
        string? name,
        DateTime? birthdate,
        string? country,
        string? city,
        Sexualities? sexuality,
        SexualOrientations? sexualOrientation,
        string? password,
        double? height,
        double? weight,
        double? latitude,
        double? longitude
        )
    {
        Id = id;
        Email = email;
        Name = name;
        Birthdate = ValidateBirthdate(birthdate);
        Country = ValidateLocation(country);
        City = ValidateLocation(city);
        Sexuality = sexuality;
        SexualOrientation = sexualOrientation;
        Password = password;
        Height = height;
        Weight = weight;
        Latitude = latitude;
        Longitude = longitude;
    } 
    
    public static User Create(
        string email,
        string? name,
        DateTime? birthdate,
        string? country,
        string? city,
        Sexualities? sexuality,
        SexualOrientations? sexualOrientation,
        string? password,
        double? height,
        double? weight,
        double? latitude,
        double? longitude)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.");

        return new User(
            Guid.NewGuid(),
            email,
            name,
            birthdate,
            country,
            city,
            sexuality,
            sexualOrientation,
            password,
            height, 
            weight, 
            latitude, 
            longitude);
    }
    
    private static string ValidateLocation(string value)
    {
        return value;
    }
    
    private static DateTime? ValidateBirthdate(DateTime? birthdate)
    {
        if (birthdate == null)
        {
            return null;
        }
        var birthdateUtc = DateTime.SpecifyKind((DateTime)birthdate, DateTimeKind.Utc);
        var today = DateTime.UtcNow;

        int age = today.Year - birthdateUtc.Year;
        if (birthdateUtc.Date > today.AddYears(-age)) age--;

        if (age < 18)
            throw new ArgumentException("User must be older than 18");

        return birthdateUtc;
    }

    public void UpdateUser(string newName, DateTime birth, SexualOrientations sexualOrientation, Sexualities sexuality, string country, string city, string password, double? height, double? weight, double? latitude, double? longitude)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name is required.");

        Name = newName;
        Birthdate = ValidateBirthdate(birth);
        Country = ValidateLocation(country);
        City = ValidateLocation(city);
        Sexuality = sexuality;
        SexualOrientation = sexualOrientation;
        Password = password;
        Height = height;
        Weight = weight;
        Latitude = latitude;
        Longitude = longitude;
    }
}