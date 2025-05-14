namespace App.Auth.DTOs;

public enum SignUpTypes
{
    SocialNetwork,
    EmailProvider,
    MailPassword
}

public class SignUpRequest
{
    public string Email { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public SignUpTypes? SignUpType { get; set; }
}