namespace Infra.Models;

public class UserView
{
    public Guid Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public DateTime? Birthdate { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Sexuality { get; set; }
    public string? SexualOrientation { get; set; }
}