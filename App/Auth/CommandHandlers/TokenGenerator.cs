using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace App.Auth.CommandHandlers;

public static class TokenGenerator
{
    public static Task<string> CreateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("XKeYqViwIeC7D5rLdmtVeae751wgTrPYFcrGfTfhL0DIzHaTtvAFZK6HuHduBpjm"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        // Define claims
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, "regular"),
        };
            // new Claim("custom_claim", "custom_value") // Claim customizada


        var token = new JwtSecurityToken(
            issuer: "flags_user.com",
            audience: "com.raphaelneves17.flags",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        // Convert the token to compact string
        var tokenHandler = new JwtSecurityTokenHandler();
        return Task.FromResult(tokenHandler.WriteToken(token));
    }
}