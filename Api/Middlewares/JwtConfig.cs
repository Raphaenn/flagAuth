using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Api.Middlewares;

public static class JwtConfig
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection serviceCollection, string key)
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
                    ValidIssuer = "flags_app",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                };
                options.RequireHttpsMetadata = false;
                
                options.Events = new JwtBearerEvents()
                {
                    
                    // O que faz: intercepta a requisição antes da validação do token, para capturar o token manualmente do header Authorization.
                    // 
                    // 📌 Isso é útil se você quiser aceitar tokens de lugares diferentes (ex: query string, cookie, etc), ou apenas logar o valor.
                    OnMessageReceived = context =>
                    {
                        // Acessar o header Authorization
                        var authorizationHeader = context.Request.Headers["authorization"].ToString();
                        // Verificar se o header contém o prefixo "Bearer " e extrair o token
                        if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
                        {
                            // Remover o prefixo "Bearer " e obter apenas o token
                            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
                        
                            // Atribuir o token ao context.Token para que ele seja validado
                            context.Token = token;
                        }
                        
                        
                        return Task.CompletedTask;
                    },
                    
                    // 👉 O que faz: esse evento dispara quando o token JWT é válido (assinatura, expiração, issuer, etc).
                    // Fazer logging
                    // Verificar se o usuário existe no banco
                    // Preencher dados extras no HttpContext.User
                    OnTokenValidated = context =>
                    {
                        // Aqui o token foi validado com sucesso
                        Console.WriteLine("✅ Token validado com sucesso!");
                        Console.WriteLine($"Usuário: {context.Principal?.Identity?.Name}");

                        // Você pode adicionar lógica extra, por exemplo, verificar se o usuário ainda existe no banco
                        return Task.CompletedTask;
                    },

                    OnAuthenticationFailed = context =>
                    {
                        // Aqui o token falhou na validação (expirado, inválido, assinatura errada, etc.)
                        Console.WriteLine("❌ Falha na autenticação:");
                        Console.WriteLine(context.Exception.Message);

                        return Task.CompletedTask;
                    },

                    OnChallenge = context =>
                    {
                        // Isso ocorre quando o token é inválido ou ausente e o endpoint requer autorização
                        Console.WriteLine("⚠️ Token ausente ou inválido ao acessar endpoint protegido");

                        return Task.CompletedTask;
                    }
                    
                };
                
            });
        serviceCollection.AddAuthorization();
        return serviceCollection;
    }
    
}