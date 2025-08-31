namespace App.Dto;

public interface ITokenService
{
    string GenerateAccessToken(Guid userId);
    string GenerateRefreshToken();
}