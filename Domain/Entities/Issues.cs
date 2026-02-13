namespace Domain.Entities;

public class Issues
{
    public Guid Id { get; set; }
    public Guid UserId { get; private set; }
    public string Content { get; private set; } = String.Empty;
    public DateTime CreatedAt { get; private set; }

    private Issues(Guid id, Guid userId, string content, DateTime createdAt)
    {
        Id = id;
        UserId = userId;
        Content = content;
        CreatedAt = createdAt;
    }

    public static Issues CreateIssue(Guid userId, string content, DateTime createdAt)
    {
        return new Issues(Guid.NewGuid(), userId, content, createdAt);
    }

    public static Issues Rehydrate(Guid id, Guid userId, string content, DateTime createdAt)
    {
        return new Issues(id, userId, content, createdAt);
    }
}