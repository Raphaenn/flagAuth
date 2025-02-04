namespace App.Auth.DTOs;

public enum SignUpTypes
{
    SocialNetwork,
    EmailProvider,
    MailPassword
}

public class SignUpRequest
{
    public string Email { get; private set; } = String.Empty;
    public string Name { get; private set; } = String.Empty;
    public SignUpTypes SignUpType { get; set; }
}