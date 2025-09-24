using Domain.Entities;

namespace Api.Dto;

public class UserPhoto
{
    public string Id { get; private set; }
    public string UserId { get; private set; }
    public string Url { get; private set; } = String.Empty;
    public DateTime UploadedAt { get; private set; }
    public bool IsProfilePicture { get; private set; }
    public string? Caption { get; private set; }
    public string? Tag { get; private set; }
}

public class GetUserResponse
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string? Name { get; set; }
    public DateTime? Birthdate { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Sexuality { get; set; }
    public string? SexualOrientation { get; set; }
    public string? Password { get; set; }
    public double? Height { get; set; }
    public double? Weight { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public UserStatus? Status { get; set; }
    
    public List<UserPhotos?>? Pics { get; set; } 
    
}