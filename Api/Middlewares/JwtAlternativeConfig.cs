using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Api.Middlewares;

public static class JwtAlternativeConfig
{
    public static IServiceCollection AddJwtAuthenticationAlternative(this IServiceCollection serviceCollection, string key)
    {
        // Get public clerk key
        var pemPath = Path.Combine(AppContext.BaseDirectory, "Keys", "publicClerk.pem");
        var publicKeyPem = File.ReadAllText(pemPath);

        // Cria uma nova instância de RSA e importa a chave pública PEM lida do arquivo.
        // Essa chave será usada para verificar a assinatura dos tokens JWT.
        var rsa = RSA.Create();
        rsa.ImportFromPem(publicKeyPem);
        
        // Cria um objeto RsaSecurityKey, que é o que o ASP.NET usa para validar assinaturas RS256 nos tokens JWT.
        var rsaKey = new RsaSecurityKey(rsa);
        
        serviceCollection.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = "https://correct-magpie-48.clerk.accounts.dev",
                    ValidAudience = "theflags.app",
                    IssuerSigningKey = rsaKey,
                    ClockSkew = TimeSpan.FromMinutes(50)
                };
            });
        serviceCollection.AddAuthorization();
        return serviceCollection;
    }
    
}