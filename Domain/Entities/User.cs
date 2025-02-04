namespace Domain.Entities;

public enum SexualOrientations
{
    Heterosexual,
    Homosexual,
    Bisexual,
}

public enum Sexualities
{
    Woman,
    Men,
}

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public DateTime? Birthdate { get; private set; }
    public string? Country { get; private set; }
    public string? City { get; private set; }
    public Sexualities? Sexuality { get; private set; }
    public SexualOrientations? SexualOrientation { get; private set; }
    
    
    public User(string name, string email)
    {
        this.Id = Guid.NewGuid();
        this.Name = name;
        this.Email = email;
    }

    internal User() {}
    
    public void ChooseSex(Sexualities sexuality)
    {
        this.Sexuality = sexuality;
    }
    
    public void SelectCountryAndCity(string country, string city)
    {
        if (string.IsNullOrEmpty(country) || string.IsNullOrEmpty(city))
        {
            throw new ArgumentException("Both Country and city must be informed");
        }
        
        this.Country = country;
        this.City = city;
    }

    public void ChooseSexualOrientation(SexualOrientations sexualOrientations)
    {
        this.SexualOrientation = sexualOrientations;
    }
    
    public void InfoBirthdate(DateTime birthdate)
    {
        DateTime today = DateTime.Today;
        int age = today.Year - birthdate.Year;
        if (birthdate.Date > today.AddYears(-age))
        {
            age--;
        }

        bool isAdult = age >= 18;
        
        if (!isAdult)
        {
            throw new ArgumentException("User must be older then 18");
        }

        this.Birthdate = birthdate;
    }
}