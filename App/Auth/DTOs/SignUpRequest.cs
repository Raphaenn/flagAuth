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
}