namespace Infra.Models;

public class UserPhotoModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Url { get; set; } = String.Empty;
    public bool IsProfilePicture { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Tag { get; set; }
    
    public UserView User { get; set; } = null!;
}