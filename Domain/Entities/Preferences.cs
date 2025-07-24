namespace Domain.Entities;

public enum Gender
{
    Heterosexual = 0,
    Homosexual = 1,
    Bisexual = 2
}

public class Preferences
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Location { get; private set; }
    public double DistanceKm { get; private set; }
    public Gender GenderPreference { get; private set; }
    public int MinAge { get; private set; }
    public int MaxAge { get; private set; }
    public double MinHeight { get; private set; }
    public double MaxHeight { get; private set; }
    public double MinWeight { get; private set; }
    public double MaxWeight { get; private set; }
    
    public List<string>? Interests { get; set; }

    private Preferences(Guid id, Guid userId, string location, double distanceKm, Gender genderPreference, int minAge, int maxAge, double minHeight, double maxHeight, double minWeight, double maxWeight)
    {
        Id = id;
        UserId = userId;
        Location = location;
        DistanceKm = distanceKm;
        GenderPreference = genderPreference;
        MinAge = minAge;
        MinAge = maxAge;
        MinHeight = minHeight;
        MaxHeight = maxHeight;
        MinWeight = minWeight;
        MaxWeight = maxWeight;
    }

    public static Preferences CreatePref(Guid userId, string location, double distanceKm, Gender genderPreference, int minAge, int maxAge, double minHeight, double maxHeight, double minWeight, double maxWeight)
    {
        return new Preferences(
            Guid.NewGuid(), 
            userId, 
            location, 
            distanceKm, 
            genderPreference, 
            minAge, 
            maxAge, 
            minHeight, 
            maxHeight, 
            minWeight, 
            maxWeight
        );
    }

    public static Preferences Rehydrate(Guid id, Guid userId, string location, double distanceKm, Gender genderPreference, int minAge, int maxAge, double minHeight, double maxHeight, double minWeight, double maxWeight)
    {
        return new Preferences(
            id, 
            userId, 
            location, 
            distanceKm, 
            genderPreference, 
            minAge, 
            maxAge, 
            minHeight, 
            maxHeight, 
            minWeight, 
            maxWeight
        );
    }
}