namespace Infra.Models;

public class IssueDbModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; } = String.Empty;
    public DateTime CreatedAt { get; set; }
}