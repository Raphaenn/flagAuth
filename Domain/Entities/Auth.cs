namespace Domain.Entities;

public class Auth
{
    public string Token { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpireAt { get; set; }

    public Auth(string token, string userId, DateTime createdAt, DateTime expireAt)
    {
        this.Token = token;
        this.UserId = userId;
        this.CreatedAt = createdAt;
        this.ExpireAt = expireAt;
    }
}