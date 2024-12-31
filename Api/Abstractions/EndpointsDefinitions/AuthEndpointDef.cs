using System.Security.Cryptography;
using App.Users.Queries;
using MediatR;
using Api.Extensions;

namespace Api.Abstractions.EndpointsDefinitions;

public class AuthEndpointDef : IEndpointsDefinitions
{
    public void RegisterEndpoints(WebApplication app)
    {
        app.MapGet("/auth/verify", async (HttpContext context, IMediator mediator) =>
        {
            string token = context.Request.Headers["authorization"];
            if (string.IsNullOrEmpty(token))
            {
                return Results.Unauthorized();
            }
            
            token = token.Substring("Bearer ".Length).Trim();

            try
            {
                RSA publicKey = PublicKeyLoader.LoadPublicKey();
                Console.WriteLine("vamos");
                Console.WriteLine(publicKey);
                var validatedClaims = JwtTokenValidator.ValidateJwtToken(token, publicKey);
                if (validatedClaims == null)
                {
                    return Results.Unauthorized();
                }

                foreach (var std in validatedClaims.Claims)
                {
                    Console.WriteLine(std);
                }
                // Extract the email claim (assuming it's named "email")
                var emailClaim = validatedClaims.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;

                Console.WriteLine(emailClaim);
                var us = new GetUserQuery
                {
                    UserId = "dsadasda",
                    Email = "raphaelnn@hotmail.com"
                };
                var post = await mediator.Send(us);
                return Results.Ok("Ok");
            }
            catch (Exception e)
            {
                return Results.Ok("Deu ruim");

                throw;
            }
           
        });
    }
}