namespace Domain.Entities;

public class UserPhotos
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Url { get; private set; } = String.Empty;
    public DateTime UploadedAt { get; private set; }
    public bool IsProfilePicture { get; private set; }

    public string? Caption { get; private set; }
    public string? Tag { get; private set; }

    private UserPhotos(Guid id, Guid userId, string url, bool isProfilePicture ,DateTime uploadedAt)
    {
        Id = id;
        UserId = userId;
        Url = url;
        IsProfilePicture = isProfilePicture;
        UploadedAt = uploadedAt;
    }

    public static UserPhotos Rehydrate(Guid id, Guid userId, string url, bool isProfilePicture, DateTime uploadedAt)
    {
        return new UserPhotos(id, userId, url, isProfilePicture, uploadedAt);
    }

    public static UserPhotos UploadPhoto(Guid userId, string url, bool isProfilePicture)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Invalid url");

        return new UserPhotos(Guid.NewGuid(), userId, url, isProfilePicture, DateTime.UtcNow);
    }
}