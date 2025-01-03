namespace Infra.Models;

public class Login
{
    public string Id { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DateTime ExpireAt { get; set; }
    public DateTime CreatedAt { get; set; }
}