namespace Api.Dto;

public class UpdateUserRequest
{
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Sexuality { get; set; }
    public string SexualOrientation { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Passsord { get; set; }
}