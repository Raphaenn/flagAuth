using Domain.Entities;

namespace Api.Dto;

public class CompleteUserRequest
{
    public string Id { get; set; }
    public string Name  { get; set; }
    public string BirthDate  { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public Sexualities Sexuality { get; set; }
    public SexualOrientations SexualOrientation { get; set; }
    public string Password { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Status { get; set; }
}