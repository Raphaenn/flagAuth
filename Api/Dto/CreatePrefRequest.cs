namespace Api.Dto;

public class CreatePrefRequest
{
    public string Location { get; set; }
    public double DistanceKm { get; set; }
    public string Orientation { get; set; }
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
    public double MinHeight { get; set; }
    public double MaxHeight { get; set; }
    public double MinWeight { get; set; }
    public double MaxWeight { get; set; }
    
    public List<string>? Interests { get; set; }
}