namespace Infra.Models;

public class Login
{
    public Guid Id { get; set; } 
    public string Token { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public DateTime ExpireAt { get; set; }
    public DateTime CreatedAt { get; set; }
}