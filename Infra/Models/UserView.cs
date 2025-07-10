using Domain.Entities;

namespace Infra.Models;

public class UserView
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string? Name { get; set; }
    public DateTime? Birthdate { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Sexuality { get; set; }
    public string? SexualOrientation { get; set; }
    public string? Password { get; set; }
    public double? Height { get; private set; }
    public double? Weight { get; private set; }
    public double? Latitude { get; private set; }
    public double? Longitude { get; private set; }
}