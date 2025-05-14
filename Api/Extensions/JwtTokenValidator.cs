using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Api.Extensions;

public static class JwtTokenValidator
{
    public static ClaimsPrincipal ValidateJwtToken(string token, RSA publicKey)
    {
        // what is it?
        var tokenHandler = new JwtSecurityTokenHandler();
        
        try
        {
            // Set up the token validation parameters
            var validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new RsaSecurityKey(publicKey),
                ValidateIssuer = false,  // Depending on your needs, you can set this to true and set the valid issuer
                ValidateAudience = false, // Similarly, set to true and define valid audience if required
                ValidateLifetime = false,  // Set this to true to validate the token's expiration
            };

            // Validate the token
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

            // If you need to check the token type or any specific claims, you can do it here
            if (validatedToken is JwtSecurityToken jwtToken)
            {
                // Optionally, you can inspect the claims or token headers here
                Console.WriteLine("Token validated successfully.");
            }

            return principal;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new Exception("Validation token failure");
        }
    }
}