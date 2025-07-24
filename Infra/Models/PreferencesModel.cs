using Domain.Entities;

namespace Infra.Models;

public class PreferencesModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Location { get; set; }
    public double DistanceKm { get; set; }
    public Gender GenderPreference { get; set; }
    public int MinAge { get; set; } = 18;
    public int MaxAge { get; set; } = 45;
    public double MinHeight { get; set; }
    public double MaxHeight { get; set; }
    public double MinWeight { get; set; }
    public double MaxWeight { get; set; }
    
    public List<string>? Interests { get; set; } 
}