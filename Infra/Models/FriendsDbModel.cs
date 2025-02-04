namespace Infra.Models;

public class FriendsDbModel
{
    public Guid Id { get; set; }
    public Guid UserId01 { get; set; }
    public Guid UserId02 { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}