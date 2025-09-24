using Domain.Abstractions;
using Domain.Events;

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

public enum UserStatus
{
    // has no name, pass
    Inactive = 0,
    // has photos
    Incomplete = 1,
    // Missing preferences
    SemiComplete = 2,
    // Completed
    Active = 3,
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
    public UserStatus? Status { get; private set; }
    
    private readonly List<IDomainEvents> _domainEvents = new();

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
        double? longitude,
        UserStatus? status
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
        Status = status;
    } 
    
    // Factory
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
        double? longitude,
        UserStatus? status
        )
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
            longitude,
            status);
    }
    
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
        double? longitude,
        UserStatus? status
        )
    {
        return new User(id, email, name, birthdate, country, city, sexuality, sexualOrientation, password, height, weight, latitude, longitude, status);
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

    public void ChangeStatus(UserStatus newStatus)
    {
        if (Status == newStatus)
            throw new ArgumentException("Invalid status change");

        if (Status == UserStatus.Inactive && newStatus == UserStatus.Incomplete)
        {
            Status = newStatus;
        }
        
        if (Status == UserStatus.Incomplete && newStatus == UserStatus.SemiComplete)
        {
            Status = newStatus;
        }

        if (Status == UserStatus.SemiComplete && newStatus == UserStatus.Active)
        {
            Status = newStatus;
        }
    }
    
    public IReadOnlyCollection<IDomainEvents> DomainEvents => _domainEvents.AsReadOnly();
    public void ClearDomainEvents() => _domainEvents.Clear();
}