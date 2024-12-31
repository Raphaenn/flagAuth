using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Api.Middlewares;

public static class JwtConfig
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = "flags_users_auth",
                };
                options.RequireHttpsMetadata = false;
                
            });
        return serviceCollection;
    }
    
}